namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Scalar object interface.
    /// </summary>
    internal abstract class ScalarObject : SnmpObject
    {
        protected ScalarObject(ObjectIdentifier id) : base(id)
        {
        }

        public override bool MatchGetNext(ObjectIdentifier id)
        {
            return Id > id;
        }

        public override bool MatchGet(ObjectIdentifier id)
        {
            return Id == id;
        }
    }
}
