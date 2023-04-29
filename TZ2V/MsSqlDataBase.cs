using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TZ2V.Data;
using TZ2V.Entity;
using TZ2V.Repositories;
using TZ2V.Repositories.ImplementingRepositories;
using TZ2V.Repositories.IRepositories;

namespace TZ2V
{
    internal class MsSqlDataBase : DataBaseConfig
    {
        #region Fields

        private readonly BrandModelRepository _brandModelRepos;
        private readonly AboutModelRepository _aboutModelRepos;
        private readonly ComplectationsRepository _complectationsRepos;
        private readonly GroupGearsRepository _groupGearsRepos;
        private readonly TileRepository _tileRepos;
        private readonly TreeInfoRepository _treeInfoRepos;
        private readonly TreeRepository _treeRepos;
        private readonly UnderTileRepository _underTileRepos;

        #endregion
        public MsSqlDataBase(string connString) : base(connString)
        {
            _brandModelRepos = new BrandModelRepository(Connection);
            _aboutModelRepos = new AboutModelRepository(Connection);
            _complectationsRepos = new ComplectationsRepository(Connection);
            _groupGearsRepos = new GroupGearsRepository(Connection);
            _tileRepos = new TileRepository(Connection);
            _treeInfoRepos = new TreeInfoRepository(Connection);
            _treeRepos = new TreeRepository(Connection);
            _underTileRepos = new UnderTileRepository(Connection);
        }


        public async Task InsertDataToDataBase(List<BrandModel> brandModels)
        {
            foreach (var brandModel in brandModels)
            {
              int brandModelid = await _brandModelRepos.Create(brandModel);
                foreach (var aboutModel in brandModel.AboutModels!)
                {
                    aboutModel.BrandModelId = brandModelid;
                    int aboutModelid = await _aboutModelRepos.Create(aboutModel);

                    
                    foreach (var complectations in aboutModel.CompleteSets)
                    {

                        int groupGearId = 0;
                        int complectid = await _complectationsRepos.Create(complectations);
                        foreach (var groupGears in complectations.GroupGears)
                        {
                            foreach (var underTile in groupGears.UnderTiles)
                            {
                                var tile = underTile.DataOnScheme;
                                int tileid = await _tileRepos.Create(tile);
                                foreach (var treeInfo in tile.Info)
                                {
                                    int treeinfoId = await _treeInfoRepos.Create(treeInfo);
                                    foreach (var tree in treeInfo.Trees)
                                    {
                                        tree.TileId = tileid;
                                        tree.TreeInfoId = treeinfoId;
                                        var treeId = await _treeRepos.Create(tree);
                                    }
                                    
                                }
                                underTile.TileId = tileid;
                                var undertileId = await _underTileRepos.Create(underTile);
                                groupGears.ComplectationsID = complectid;
                                groupGears.UnderTileId = undertileId;
                                groupGearId = await _groupGearsRepos.Create(groupGears);
                            }                                  
                           
                        }
                        await InsertModelAndComlectations(complectid, aboutModelid);
                    }

                }             
            }
                                 
        }
        private async Task InsertModelAndComlectations(int idComplect,int aboutModelId)
        {
            var command = Connection.CreateCommand();

            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "SP_InsertDataToModelAndComplectations";

            command.Parameters.Add(new SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.Int,
                SqlValue = idComplect,
                ParameterName = "@ComplectatinsID",
                Direction = System.Data.ParameterDirection.Input
                
            });

            command.Parameters.Add(new SqlParameter
            {
                SqlDbType = System.Data.SqlDbType.Int,
                SqlValue = aboutModelId,
                ParameterName = "@AboutModelID",
                Direction = System.Data.ParameterDirection.Input

            });

            command.Parameters.Add(new SqlParameter
            {
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
                ParameterName = "@IdInsertedItem",
            });

            await command.ExecuteNonQueryAsync();

        }


    }
}
