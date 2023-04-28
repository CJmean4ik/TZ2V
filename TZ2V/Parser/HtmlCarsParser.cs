using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TZ2V.Data;
using TZ2V.Entity;

namespace TZ2V.Parser
{
    /// <summary>
    /// Парсер для парсинг сайта www.ilcats.ru
    /// </summary>
    internal class HtmlCarsParser : ParserConfig, IParser<List<BrandModel>>
    {
        public static HttpClient? httpClient { get; set; }
        public string DirToFiles { get; set; }

        #region Fileds
        private IConfiguration _config;
        private IDocument _doc;
        private IBrowsingContext _context;
        private int indexComplectations;
        private int indexUnderTile;
        #endregion

        /// <summary>
        /// Устанавливает тип парсера 
        /// </summary>
        /// <param name="typeConfig">Тип парсера(Считать с файла или по URL)</param>
        /// <param name="uri">Ссылка на сайт</param>
        /// <param name="countComplectations">Количество выбераемых комплектаций</param>
        /// <param name="indexUnderTile">Количество выбираемых под-деталей</param>
        public HtmlCarsParser(ConfigurationType typeConfig, Uri uri,string dir, int countComplectations = 1, int indexUnderTile = 1) : base(uri)
        {
            _config = typeConfig == ConfigurationType.Default ?
               Configuration.Default :
               Configuration.Default.WithDefaultLoader();
            _context = BrowsingContext.New(_config);
            BaseUri = uri;
            httpClient = new HttpClient();
            indexComplectations = countComplectations;
            this.indexUnderTile = indexUnderTile;
            DirToFiles = dir;
        }

        /// <summary>
        /// Парсит n количество моделей определённой марки.
        /// <br/>Создает и заполняет все сущности по цепочке:
        ///<br/>AboutModel
        ///<br/>Complectations
        ///<br/>GroupGears
        ///<br/>Tile
        ///<br/>Tree
        ///<br/>TreeInfo
        ///<br/>UnderTile
        /// </summary>
        /// <param name="indexTake"></param>
        /// <returns></returns>
        public async Task<List<BrandModel>> ParseAsync(int indexTake)
        {
            using (_doc = await _context.OpenAsync(BaseUri.AbsoluteUri))
            {
                //Выбираем количество моделей марки TOYOTA
                var list = _doc.QuerySelectorAll("div.Multilist > div.List").Take(indexTake).ToList();
                List<BrandModel> propcar = new List<BrandModel>();

                //Цикл, который будет создавать и заполнять обьект newcar типа BrandModel. 
                for (int i = 0; i < list.Count; i++)
                {
                    BrandModel newcar = new BrandModel();


                    //Выбираем имя модели и присваиваем его в newcar.NameModel
                    newcar.NameModel = list[i].QuerySelector("div.Header > div.name")?.TextContent;

                    //Используем выборку, для определения количестова строк, которые содержат информацию о модели
                    var data = list[i].QuerySelector("div.List");
                    var modelCar = data.QuerySelectorAll("div.List").ToList();


                    for (int j = 0; j < data.ChildElementCount; j++)
                    {
                        //Выбираем ссылку для следующей таблици - комплектация 
                        var hrefPath = modelCar[j].QuerySelector("a").GetAttribute("href");

                        //Создаем AboutModel и заполняем его информацией про модель и добовляем в newcar.AboutModels
                        newcar.AboutModels!.Add(new AboutModel
                        {
                            Code = modelCar[j].QuerySelector("div.id")?.TextContent.Trim(),
                            UrlCompleteSets = BaseUrl + hrefPath,
                            DataRange = modelCar[j].QuerySelector("div.dateRange")?.TextContent.Trim(),
                            CodeModel = modelCar[j].QuerySelector("div.modelCode")?.TextContent.Trim()
                        });


                        await ParseComplectationsByUrlAsync(newcar.AboutModels[j].UrlCompleteSets, newcar.AboutModels[j]);
                    }
              
                    propcar.Add(newcar);
                }

                return propcar;
            }
        }

        /// <summary>
        /// Парсит определёное количество комплектаций, которые относятся к определённой модели 
        /// </summary>
        /// <param name="codeUrl"></param>
        /// <param name="car"></param>
        /// <returns></returns>
        private async Task ParseComplectationsByUrlAsync(string codeUrl, AboutModel car)
        {
            using (_doc = await _context.OpenAsync(codeUrl))
            {
                //Выбираем первую таблицу на странице, содержащую информацию о комплектациях
                var table = _doc.QuerySelectorAll("#Body > table").Take(1).ToList();
                         
                for (int i = 0; i < table.Count; i++)
                {

                    //Выбираем количество строк, которые содержат инф. про комплектацию
                    var tr = table[i].QuerySelectorAll("tbody > tr").Skip(1).Take(indexComplectations).ToList();
                    //Списко названий комплектаций. (Заголовки в таблице)
                    var header = table[i].QuerySelectorAll("tbody > tr").Take(1).ToList();


                    for (int j = 0; j < tr.Count; j++)
                    {

                        //Выбираю те элемент, у которых свойство TextContent не пустое
                        var td = tr[j].Children.Where(w => w.TextContent != "").ToList();

                        //Выбираю определёное количетство заголовков, которые соответсвуют не пустым элементам
                        var head = header[0].Children.Take(td.Count).ToList();
                        
                        var set = CreateCompleteSet(td,head);

                       await ParseGroupGearsByUrlAsync(set.GroupGearsUrl, set);

                       car.CompleteSets!.Add(set);
                    }

                }

            }

        }

        /// <summary>
        /// Парсит, создает и заполняет группы деталей GroupGears
        /// </summary>
        /// <param name="ComplectationsUrl"></param>
        /// <param name="completeSets"></param>
        /// <returns></returns>
        private async Task ParseGroupGearsByUrlAsync(string ComplectationsUrl, Complectations completeSets)
        {
            using (_doc = await _context.OpenAsync(ComplectationsUrl))
            {
                //Получаем список из 4 деталей 
                var gears = _doc.QuerySelectorAll("div.List  div.List").ToList();

                //Заполняем их
                for (int i = 0; i < gears.Count; i++)
                {
                    GroupGears groupGear = new GroupGears();
                    //Получаем ссылку на под-деталь этой делати и название этой детали
                    groupGear.UrlOnGears = BaseUrl + gears[i]?.QuerySelector("div.name > a")?.GetAttribute("href");
                    groupGear.NameGroup = gears[i]?.QuerySelector("div.name")?.TextContent;

                  await ParseUnderTileByUrlAsync(groupGear.UrlOnGears, groupGear);
                  completeSets.GroupGears.Add(groupGear);
                }
            }
        }

        /// <summary>
        /// Парсит и заполняет под-деталь и потом связывает её с деталью
        /// </summary>
        /// <param name="GearsUrl"></param>
        /// <param name="groupGear"></param>
        /// <returns></returns>
        private async Task ParseUnderTileByUrlAsync( string GearsUrl, GroupGears groupGear)
        {
            using (_doc = await _context.OpenAsync(GearsUrl))
            {
                //Получаем n количество под-деталей, детали
                var list = _doc.QuerySelectorAll("div.Tiles > div.List ").ToList();
                var tiles = list[0].QuerySelectorAll("div.List").Take(indexUnderTile).ToList();

                UnderTile underTile = new UnderTile();
                for (int i = 0; i < tiles.Count; i++)
                {
                    //Записываем название под-детали и ссылку на страницу с информацией о ней 
                    underTile.NameTile = tiles[i].QuerySelector("div.name")!.TextContent;
                    underTile.DataTileUrl = BaseUrl + tiles[i].QuerySelector("div.name > a")!.GetAttribute("href");

                   await ParseInfoAboutTilesAsync(underTile.DataTileUrl, underTile, underTile.NameTile);
                   groupGear.UnderTiles.Add(underTile);
                }

            }
        }

        /// <summary>
        /// Парсит и заполняет информацию о под-детали 
        /// </summary>
        /// <param name="underGroupUrl"></param>
        /// <param name="underTile"></param>
        /// <returns></returns>
        private async Task ParseInfoAboutTilesAsync(string underGroupUrl, UnderTile underTile,string imageName)
        {
            using (_doc = await _context.OpenAsync(underGroupUrl))
            {
                //Получаем таблицу с данными о под-детали
                var table = _doc.QuerySelectorAll("div.Info > table > tbody").ToList();
                var data_id = table[0].QuerySelectorAll("tr[data-id]")?.ToList();


                Tile tile = new Tile();

                //Считываем изображение поддетали и сохраняем её
                await DownloadImageAsync(tile,imageName);

                for (int i = 0; i < data_id.Count;)
                {
                    TreeInfo treeInfo = new TreeInfo(); 
                    
                    //Разбиваем заголовок на данные и получаем обратно список под-данных 
                    var data = SplitingTreeNameAndBind(treeInfo, data_id[i], table[0]);
                    i++;

                    //Пропускаем в таблице заголовки
                    var list = data.Skip(1).ToList();


                    CreateTreeInfo(list,treeInfo,ref i);

                    tile.Info.Add(treeInfo);

                   
                  
                }
                underTile.DataOnScheme = tile;
            }
        }

        /// <summary>
        /// Заполняет и возвращает обьект типа Complectations, который содержит информацию про комплектацию
        /// </summary>
        /// <param name="td"></param>
        /// <param name="headers"></param>
        /// <returns>Complectations</returns>
        private Complectations CreateCompleteSet(List<IElement> td,List<IElement> headers)
        {
            //Выбираем код и ссылку для следующей выборки по запчастям
            Complectations complete = new Complectations();
            complete.Code = td[0].QuerySelector("div > a").TextContent;
            complete.GroupGearsUrl = BaseUrl + td[0].QuerySelector("div > a")?.GetAttribute("href");


            int count = td.Count;

            //С помощью рефлексии получаю количество свойств класса Complectations и пропускаю первых два: Code и GroupGearsUrl
            Type type = complete.GetType();
            var propertie = type.GetProperties().SkipLast(2).ToList();

            for (int i = 0; i < count; i++)
            {
                //Пропускаю эти данные, так как Code у нас уже есть, а Дата в выборку не должна попадать
                if (propertie[i].Name == "Code" || headers[i].TextContent == "Дата")            
                    continue;

                //Получаю название заголовка, к которому относится его информация.
                //Н-р: заголовок ENGINE 1 и его значени первой строки: 2L

                var name = headers[i].TextContent.Contains(" ") ? 
                    headers[i].TextContent.Replace(" ","").ToLower()
                    : headers[i].TextContent.Replace(",", "").ToLower();


                //Выбираем то свойство у класса Complectations, у которого название совпадает с заголовком 
                for (int j = 1; j < propertie.Count; j++)
                {
                    if (propertie[j].Name.ToLower() == name)
                    {
                        //Присваиваем ему значение и выходим из цикла 
                        propertie[j].SetValue(complete, td[i].TextContent);
                        break;
                    }
                }                            
            }

            return complete;
        }

        /// <summary>
        /// Парсит и создает информцию про под-деталь
        /// </summary>
        /// <param name="list"></param>
        /// <param name="treeInfo"></param>
        /// <param name="i"></param>
        private void CreateTreeInfo(List<IElement> list, TreeInfo treeInfo,ref int i)
        {
            
            for (int j = 0; j < list.Count(); j++)
            {
                Tree treeD = new Tree();
                //Выбираме номера у под-деталей в таблице 
                var replaceNumber = list[0].QuerySelector("div.replaceNumber > a");
                var nonReplaceNumber = list[0].QuerySelector("div.number > a");

                string number = replaceNumber == null ? nonReplaceNumber.TextContent : replaceNumber.TextContent;



                var dataInfo = list[j].Children;
                //Устанавливаем в treeD следующие данные столбцов из таблици: Номер (код), Кол-во, Дата и Применяемость
                treeD.Code = number;
                treeD.Count = int.TryParse(dataInfo[1].TextContent, out int count) ? count : 0;
                treeD.Data = dataInfo[2].TextContent;
                treeD.Info = dataInfo[3].TextContent;

                i++;
                treeInfo.Trees.Add(treeD);
            }
        }

        /// <summary>
        /// Парсим и разбиваем имя и код подданных
        /// </summary>
        /// <param name="treeInfo"></param>
        /// <param name="dataId"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<IElement> SplitingTreeNameAndBind(TreeInfo treeInfo,IElement dataId,IElement table)
        {
            //Получаем имя класса и выбираем только те элементы, у которых имя класса == className
            var className = dataId.ClassName;
            var data = table.QuerySelectorAll($"tr[class='{className}']").ToList();
            var treeCode = new String(data[0].QuerySelector("th")?.TextContent.Where(Char.IsDigit).ToArray());
            var tree = data[0].QuerySelector("th")?.TextContent.Replace(treeCode, "");

            treeInfo.TreeCode = treeCode;
            treeInfo.TreeName = tree;

            return data;
        }

        /// <summary>
        /// Скачивает изображение-схему под-детали
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        private async Task DownloadImageAsync(Tile tile,string imageName)
        {
            var imageUrl = _doc.QuerySelector("div.ImageArea > div.Image > img")?.GetAttribute("src");

            tile.ImageScheme = await httpClient.GetByteArrayAsync("https:" + imageUrl);
            string imageDir = DirToFiles + $"{imageName}.jpg";

            if (File.Exists(imageDir)) return;

            using var stream = new MemoryStream(tile.ImageScheme);
            using var file = File.OpenWrite(imageDir);
            await stream.CopyToAsync(file);
            stream.Position = 0;
        }
    }
    enum ConfigurationType
    {
        Default,
        Loader
    }
}
