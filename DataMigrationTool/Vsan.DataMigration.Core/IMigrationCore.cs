using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.DataMigration.Core
{
    public interface IMigrationCore
    {

        /// <summary>
        ///  开始数据迁移
        /// </summary>
        /// <param name="workOrderId">工单编号</param>
        /// <returns></returns>
        int StartMigration(string workOrderId);


        /// <summary>
        /// 停止数据迁移
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        int StopMigration(string workOrderId);

    }
}
