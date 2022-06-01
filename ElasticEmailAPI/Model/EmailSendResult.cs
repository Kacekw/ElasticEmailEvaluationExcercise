using ElasticEmail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticEmailAPI.Model
{
    public class EmailSendResult
    {
        public string TransactionID { get; private set; }
        public string MessageID { get; private set; }

        internal EmailSendResult(string TransactionId, string MessageId)
        {
            TransactionID = TransactionId;
            MessageID = MessageId;
        }

        public override string? ToString()
        {
            return $"Transaction ID : {TransactionID}" + Environment.NewLine + $"Message ID : {MessageID}";
        }
    }
}
