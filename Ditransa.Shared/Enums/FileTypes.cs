using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ditransa.Shared.Enums
{
    public sealed class FileTypes
    {
        public static FileTypes ProfileImage { get; } = new FileTypes(1, nameof(ProfileImage), "Profiles");
        public static FileTypes GroupFile { get; } = new FileTypes(2, nameof(GroupFile), "Groups");
        public static FileTypes ClientFile { get; } = new FileTypes(3, nameof(ClientFile), "Clients");
        public static FileTypes AccountFile { get; } = new FileTypes(4, nameof(AccountFile), "Accounts");
        public static FileTypes KickbackFile { get; } = new FileTypes(5, nameof(KickbackFile), "Kickbacks");
        public static FileTypes ProposalFile { get; } = new FileTypes(6, nameof(ProposalFile), "Proposals");
        public static FileTypes ProposalOtherDocumentFile { get; } = new FileTypes(7, nameof(ProposalOtherDocumentFile), "Proposals");
        public static FileTypes ProposalLineItemFile { get; } = new FileTypes(8, nameof(ProposalLineItemFile), "ProposalLineItems");


        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Value { get; private set; }

        private FileTypes(int id, string name, string value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        public static IEnumerable<FileTypes> List()
        {
            // alternately, use a dictionary keyed by value
            return new[] { ProfileImage, GroupFile, ClientFile };
        }

        public static FileTypes FromId(int id)
        {
            return List().Single(r => r.Id == id);
        }

        public static FileTypes FromName(string name)
        {
            return List().Single(r => r.Name == name);
        }
    }
}
