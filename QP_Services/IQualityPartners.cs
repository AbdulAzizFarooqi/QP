using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Data.Odbc;
using DBHelper;
using System.ServiceModel.Activation;
using Microsoft.SharePoint.Client;
using System.Net;
using System.Web.Script.Serialization;
using System.Collections;
using System.ServiceModel.Web;
namespace QP_Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IQualityPartners" in both code and config file together.
    [ServiceContract]
    public interface IQualityPartners
    {
        [OperationContract]
        void DoWork();
         #region Labeling Module
         [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(string))]
         string   GetStickers(string InspectorsName,string ParamServiceName);
        #endregion

        #region Quality Partner Module
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(string))]
        List<QualityPartnerDocs> GetDocumentsStatus(string ParterId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(string))]
        QualityPartner GetDetailsInformatoin(string ParterId);



         [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(string))]
        QualityPartner GetDetailsInformatoinFinalStage(string ParterId);





        

        #endregion


    }

    #region Quality Partnemt
    [DataContract()]
    public class QualityPartnerDocs
    {

        [DataMember]
        public string DocumentId { get; set; }
        [DataMember]
        public string DocumentName { get; set; }
        [DataMember]
        public string Uploaded { get; set; }
        [DataMember]
        public string View { get; set; }
        [DataMember]
        public string Pending { get; set; }
        [DataMember]
        public string CurrentStage { get; set; }

    }

    [DataContract()]
    public class QualityPartner
    {

        [DataMember]
        public string DocumentId { get; set; }
        [DataMember]
        public string DocumentName { get; set; }
        [DataMember]
        public string Uploaded { get; set; }
        [DataMember]
        public string View { get; set; }
        [DataMember]
        public string Pending { get; set; }

        [DataMember]
        public string SiteEvaluatoinComments { get; set; }

        [DataMember]
        public string SiteEvalution { get; set; }

        [DataMember]
        public string TradeLicense { get; set; }

        [DataMember]
        public string FactoryName { get; set; }

        [DataMember]
        public string Address { get; set; }


        [DataMember]
        public string TradeLiecense { get; set; }

        [DataMember]
        public string OwnerName { get; set; }
        [DataMember]
        public string TelephoneNumber { get; set; }
        [DataMember]
        public string RequestCatagory { get; set; }

    }


    #endregion

       





}
