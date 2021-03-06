﻿using System;
using System.Collections.Generic;
using System.Linq;
using Medivh.Content;
using Medivh.Models;

namespace Medivh.DataStorage.OnceData.Counter
{
    internal class CoustomCounter : OnceDataStorage
    {
        private static DataCache cache = new DataCache(ModuleTypeEnum.OnceData, CounterTypeEnum.Custom);
        public override void Add(BaseModel model)
        {
            cache.Add(model);
        }
 

//        public override IList<BaseModel> Get(Predicate<BaseModel> match)
//        {
//            return cache.Get(match);
//        }

//
//        public override IList<BaseModel> Get(CmdModel cmd)
//        {
//            if (cmd.Operate == "types")
//            {
//                return this.GetTypes();
//            }
//            else
//            {
//                return base.Get(cmd);
//            }
//        }


//        private IList<BaseModel> GetTypes()
//        {
//            var list = cache.GetAll().ToList();
//            var slist = list.Select(x => x.Mark).Distinct().ToList();
//            if (slist.Count == 0)
//            {
//                return new List<BaseModel>();
//            }
//            return slist.Select(s => new BaseModel() {Mark = s}).ToList();
//        }
         
        public override DataCache GetDataCache()
        {
            return cache;
        }
    }
}
