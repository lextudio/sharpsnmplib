using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// GET BULK message handler.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class GetBulkMessageHandler : IMessageHandler
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="store">The object store.</param>
        /// <returns></returns>
        public ResponseData Handle(SnmpContext context, ObjectStore store)
        {
            // TODO: implement this to conform to RFC.
            IList<Variable> result = new List<Variable>();
            Variable v = context.Request.Pdu.Variables[0];

            Variable temp = v;
            int total = context.Request.Pdu.ErrorIndex.ToInt32();
            while (total-- > 0)
            {
                ScalarObject next = store.GetNextObject(temp.Id);
                if (next == null)
                {
                    temp = new Variable(temp.Id, new EndOfMibView());
                    result.Add(temp);
                    break;
                }

                // TODO: how to handle write only object here?
                temp = next.Variable;
                result.Add(temp);
            }

            return new ResponseData(result, ErrorCode.NoError, 0);
        }
    }
}