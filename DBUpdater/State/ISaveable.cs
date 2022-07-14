namespace DBUpdater.State
{
    interface ISaveable
    {
        void Save(IWriter writer, string name);
        void Load(IWriter writer, string name);
    }
}
