using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.DomainModel.DomainObjects
{
   public class VedludbPostModel
    {
        public string menesis { get; set; }
        public string callbackUrl { get; set; }
        public List<nodarbinatieArray> nodarbinatie { get; set; }
       // public List<dienasArray> dienas { get; set; }
       // public List<ierakstiArray> ieraksti { get; set; }

        public VedludbPostModel()
        {
            menesis = "";
            callbackUrl = "";
            nodarbinatie = new List<nodarbinatieArray>();
            //dienas = new List<dienasArray>();
            //ieraksti = new List<ierakstiArray>();

        }
    }


    public class nodarbinatieArray
    {
        public string vards { get; set; }
        public string uzvards { get; set; }
        public string personasKods { get; set; }
        public string arvalstniekaId { get; set; }
        public string vizasNr { get; set; }
        public string uzturesanasAtlaujasNr { get; set; }
        public string dzimsanasDatums { get; set; }
        public string amats { get; set; }
        public string darbaDevejaNosaukums { get; set; }
        public string darbaDevejaRegistracijasNumurs { get; set; }
        public string arvalstuDarbaDevejs { get; set; }
        public string summaraisMenesa { get; set; }
        public string faktiskaisSummaraisMenesa { get; set; }

       public List<dienasArray> dienas { get; set; }
       public List<ierakstiArray> ieraksti { get; set; }

        public nodarbinatieArray()
        {
            vards = "";
            uzvards = "";
            personasKods = "";
            arvalstniekaId = "";
            vizasNr = "";
            uzturesanasAtlaujasNr = "";
            dzimsanasDatums = "";
            amats = "";
            darbaDevejaNosaukums = "";
            darbaDevejaRegistracijasNumurs = "";
            arvalstuDarbaDevejs = "";
            summaraisMenesa = "";
            faktiskaisSummaraisMenesa = "";

            dienas = new List<dienasArray>();
            ieraksti = new List<ierakstiArray>();
        }
    }

    public class dienasArray
    {
        public string datums { get; set; }
        public string summaraisDiena { get; set; }
        public string faktiskaisSummaraisDiena { get; set; }

        public dienasArray()
        {
            datums = "";
            summaraisDiena = "";
            faktiskaisSummaraisDiena = "";
        }
    }

    public class DienaWithId
    {
        public string Id { get; set; }
        public string datums { get; set; }
        public string summaraisDiena { get; set; }
        public string faktiskaisSummaraisDiena { get; set; }
    }

        public class ierakstiArray
    {
        public string ierasanasLaiks { get; set; }
        public string iziesanasLaiks { get; set; }
        public string faktiskaisIerasanasLaiks { get; set; }
        public string faktiskaisIziesanasLaiks { get; set; }
        public string pamatojums { get; set; }
        public string manualsIerasanasIeraksts { get; set; }
        public string manualsIziesanasIeraksts { get; set; }
        public string autonomsIerasanasIeraksts { get; set; }
        public string autonomsIziesanasIeraksts { get; set; }

        public ierakstiArray()
        {
            ierasanasLaiks = "";
            iziesanasLaiks = "";
            
            faktiskaisIerasanasLaiks = "";
            faktiskaisIziesanasLaiks = "";
            pamatojums = "";
            manualsIerasanasIeraksts = "";
            manualsIziesanasIeraksts = "";
            autonomsIerasanasIeraksts = "";
            autonomsIziesanasIeraksts = "";
        }
    }
    public class IerakstiWithId
    {
        public string Id { get; set; }
        public string ierasanasLaiks { get; set; }
        public string iziesanasLaiks { get; set; }
        public string faktiskaisIerasanasLaiks { get; set; }
        public string faktiskaisIziesanasLaiks { get; set; }
        public string pamatojums { get; set; }
        public string manualsIerasanasIeraksts { get; set; }
        public string manualsIziesanasIeraksts { get; set; }
        public string autonomsIerasanasIeraksts { get; set; }
        public string autonomsIziesanasIeraksts { get; set; }
    }

    }
