namespace Lextm.SharpSnmpLib.Mib
{
    public class SimpleObjectRegistry : ObjectRegistryBase
    {
        public SimpleObjectRegistry()
        {
            Tree = new ObjectTree();
        }
    }
}
