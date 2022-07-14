using CECommon.DataAccess;

namespace DBUpdater
{
    public interface IContext
    {
        CEContext AsLegacyImplementer();
        /*
using CEProvider = CECommon.DataAccess.CEProvider;
using DBSettings = CECommon.DataAccess.DBSettings;

            var dbProvider = CEProvider.CreateProvider(DBSettings.DBDriver, DBSettings.DBProvider, DBSettings.DBServer, DBSettings.DBName, DBSettings.DBTimeout, DBSettings.DBIsolationLevel);
            var ctx = dbProvider.CreateContext(DBSettings.DBUser, DBSettings.DBPassword, null);
        */
    }
}