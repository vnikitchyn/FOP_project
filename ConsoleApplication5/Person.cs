using System.Text;

namespace FOPcsvToSQLApp
{
    partial class Name
    {    
        #region autoproperties of Name
        public string Surname { get; set; }
        public string NameFirst { get; set; }
        public string Father { get; set; }        
        #endregion
        public Name(string nameFirst, string surname, string father)
        {
            NameFirst = nameFirst;
            Father = father;
            Surname = surname;
        }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("fisrt_name:").Append(NameFirst)
            .Append(" surname:").Append(Surname).
            Append(" father_name:").Append(Father);
            return sb.ToString();
        }
    }

    partial class Adress
    {
        #region getters and setters of Adress
        public string Region { get; set; }
        public string AdressString { get; set; }
        public int Index { get; set; }
        #endregion
        public Adress(int index, string region, string adressString)
        {
            Index = index;
            Region = region;
            AdressString = adressString;
        }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("region:").Append(Region)
           .Append(" AdressString:").Append(AdressString)
           .Append(" index:").Append(Index);

            return sb.ToString();
        }

    }

    enum Status
    { Disabled, Active }

    partial class Person
    {
        #region AutoProperties
        public int Id { get; set; }
        private string Type { get; set; }
        private string Status { get; set; }
        private Name Name { get; set; }
        private Adress Adress { get; set; }
        #endregion


        public Person(int id, Name name, string type, string status, Adress adress)
        {
            Id = id;
            Type = type;
            Status = status;
            Name = name;
            Adress = adress;
        }
        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("id:").Append(Id)
              .Append(" type:").Append(Type)
              .Append(" status:").Append(Status)
              .Append(" ").Append(Name)
              .Append(" ").Append(Adress);
            return sb.ToString();
        }
    }


}
