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

namespace QP_Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "QualityPartners" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select QualityPartners.svc or QualityPartners.svc.cs at the Solution Explorer and start debugging.


    [AspNetCompatibilityRequirements(RequirementsMode =
      AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehaviorAttribute(IncludeExceptionDetailInFaults = true)]
    public class QualityPartners : IQualityPartners
    {
        string ProductionFlag = System.Configuration.ConfigurationSettings.AppSettings["ProductionFlag"];
        string url = "";
        public void QualityPartner()
        {
            if (ProductionFlag=="YES")
            {
                url="http://apps.qcc.abudhabi.ae/Inspections"; 
            }
            else
            {
                url = "http://sp-app2-dev:35897/VIS-DEV/"; 
            }

        }
        public void DoWork()
        {
        }
        #region Quality PartN
        public List<QualityPartnerDocs> GetDocumentsStatus(string ParterId)
        {
            // ParterId = "1";
            QualityPartner();
            ArrayList _Array = new ArrayList();
            List<QualityPartnerDocs> MainList = new List<QualityPartnerDocs>();
            //string url = "";
            ClientContext clientContext = new ClientContext(url);
            NetworkCredential credentials = new NetworkCredential("bot1", "12345678", "ADQCC");
            DataTable _Table = new DataTable();
            var ViewLink = "http://sp-app2-dev:35897/";

            clientContext.Credentials = credentials;
            Microsoft.SharePoint.Client.List spList = clientContext.Web.Lists.GetByTitle("QualityPartnerDocuments");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();
            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                   @"<View>          <Query>      <Where><Eq><FieldRef Name='PartnerID' /><Value Type='Text'>" + ParterId + "</Value></Eq></Where>           </Query>     <FieldRef Name='FileRef' /> <FieldRef Name='ID' />  <FieldRef Name='SupportDocumentId' /></ViewFields>                               </View>";
                ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();

                _Table.Columns.Add("ID");
                _Table.Columns.Add("Name");

                foreach (ListItem item in listItems)
                {
                    string UploadedDocumentID = Convert.ToString(item["SupportDocumentId"]);
                    _Array.Add(UploadedDocumentID);
                    DataRow _ravi = _Table.NewRow();
                    _ravi["ID"] = Convert.ToInt32(item["SupportDocumentId"]);
                    _ravi["Name"] = Convert.ToString(item["FileRef"]); ;
                    _Table.Rows.Add(_ravi);

                }
            }




            clientContext.Credentials = credentials;
            spList = clientContext.Web.Lists.GetByTitle("QualityPartnerSupportingDocs");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();
            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                   @"<View>          <Query>      <Where><Gt><FieldRef Name='ID' /><Value Type='Counter'>0</Value></Gt></Where>           </Query>    <FieldRef Name='ID' />  <FieldRef Name='Title' /></ViewFields>                               </View>";
                ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();



                foreach (ListItem item in listItems)
                {
                    QualityPartnerDocs _p = new QualityPartnerDocs();
                    _p.DocumentName = Convert.ToString(item["Title"]);
                    _p.DocumentId = Convert.ToString(item["ID"]);
                    if (_Array.Contains(_p.DocumentId))
                    {
                        DataRow[] foundRows;

                        foundRows = _Table.Select("ID=" + _p.DocumentId + "");
                        foreach (DataRow row in foundRows)
                        {
                            //_p.DocumentName = Convert.ToString(row[1]);
                            _p.View = ViewLink + Convert.ToString(row[1]); ;// ViewLink + "/" + _p.DocumentName;
                        }
                        _p.Uploaded = "Uploaded";
                    }
                    else
                    {
                        _p.Uploaded = "Not";
                        _p.View = "";
                    }


                    MainList.Add(_p);


                }
            }

            if (ParterId == null)
            {
                ParterId = "1";
            }

            //url = "http://sp-app2-dev:35897/VIS-DEV/";
            clientContext = new ClientContext(url);
            credentials = new NetworkCredential("bot1", "12345678", "ADQCC");
            clientContext.Credentials = credentials;
            spList = clientContext.Web.Lists.GetByTitle("QualityPartners");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();
            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                   @"<View>          <Query>      <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + ParterId + "</Value></Eq></Where>           </Query>      <FieldRef Name='ID' />"
                   + "<FieldRef Name='SupportDocumentId' /></ViewFields>"
                    + "<FieldRef Name='TradeLiecense' /></ViewFields>"
                   + "<FieldRef Name='EntityFactoryName' /></ViewFields>"
                   + "<FieldRef Name='OwnerName' /></ViewFields>"
                   + "<FieldRef Name='TelephoneNumber' /></ViewFields>"
                   + "<FieldRef Name='CurrentStage' /></ViewFields>"
                  + " </View>";
                ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();

                foreach (ListItem item in listItems)
                {
                    //  Partner.TradeLicense = Convert.ToString(item["CurrentStage"]);
                    MainList[0].CurrentStage = Convert.ToString(item["CurrentStage"]);


                }
            }




            return MainList;

        }
        public QualityPartner GetDetailsInformatoin(string ParterId)
        {

            List<QualityPartnerDocs> GetDocumentsStatusOb = new List<QualityPartnerDocs>();
            QualityPartner();
            // ParterId = "3";
            ArrayList _Array = new ArrayList();
            QualityPartner Partner = new QualityPartner();
            string url = "http://sp-app2-dev:35897/VIS-DEV/";
            ClientContext clientContext = new ClientContext(url);
            NetworkCredential credentials = new NetworkCredential("bot1", "12345678", "ADQCC");
            DataTable _Table = new DataTable();
            var ViewLink = "http://sp-app2-dev:35897/";

            clientContext.Credentials = credentials;
            Microsoft.SharePoint.Client.List spList = clientContext.Web.Lists.GetByTitle("QualityPartners");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();
            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                   @"<View>          <Query>      <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + ParterId + "</Value></Eq></Where>           </Query>      <FieldRef Name='ID' />"
                   + "<FieldRef Name='SupportDocumentId' /></ViewFields>"
                    + "<FieldRef Name='TradeLiecense' /></ViewFields>"
                   + "<FieldRef Name='EntityFactoryName' /></ViewFields>"
                   + "<FieldRef Name='OwnerName' /></ViewFields>"
                   + "<FieldRef Name='TelephoneNumber' /></ViewFields>"
                   + "<FieldRef Name='RequestCatagory' /></ViewFields>"
                  + " </View>";
                ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();

                foreach (ListItem item in listItems)
                {
                    Partner.TradeLicense = Convert.ToString(item["TradeLiecense"]);
                    Partner.FactoryName = Convert.ToString(item["EntityFactoryName"]);
                    Partner.OwnerName = Convert.ToString(item["OwnerName"]);
                    Partner.TelephoneNumber = Convert.ToString(item["TelephoneNumber"]);
                    Partner.RequestCatagory = Convert.ToString(item["RequestCatagory"]);

                }
            }


            //</Neq><FieldRef Name='SupportDocumentId' /><Value Type='Text'>FinalApproval</Value></Neq>

            clientContext.Credentials = credentials;
            spList = clientContext.Web.Lists.GetByTitle("QualityPartnerSupportingDocs");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();
            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                     @"<View>          <Query>      <Where><Gt><FieldRef Name='ID' /><Value Type='Counter'>0</Value></Gt></Where>           </Query>    <FieldRef Name='ID' />  <FieldRef Name='Title' /></ViewFields>                               </View>";
                ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();

                _Table.Columns.Add("ID");
                _Table.Columns.Add("Title");

                foreach (ListItem item in listItems)
                {
                    //string UploadedDocumentID = Convert.ToString(item["ID"]);
                    // _Array.Add(UploadedDocumentID);
                    DataRow _ravi = _Table.NewRow();
                    _ravi["ID"] = Convert.ToInt32(item["ID"]);
                    _ravi["Title"] = Convert.ToString(item["Title"]); ;
                    _Table.Rows.Add(_ravi);

                }
            }
            string Documents = "";
            string DocumentsNames = "";

            clientContext.Credentials = credentials;
            spList = clientContext.Web.Lists.GetByTitle("QualityPartnerDocuments");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();
            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                   @"<View>          <Query>      <Where><And><Eq><FieldRef Name='PartnerID' /><Value Type='Text'>" + ParterId + "</Value></Eq><Neq><FieldRef Name='SupportDocumentId' /><Value Type='Text'>FinalApproval</Value></Neq></And></Where>           </Query>    <FieldRef Name='SupportDocumentId' /> <FieldRef Name='ID' />  <FieldRef Name='FileRef' /></ViewFields>                               </View>";
                ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();

                foreach (ListItem item in listItems)
                {
                    Documents += Convert.ToString(item["FileRef"]) + "*";
                    string SupportDocumentId = Convert.ToString(item["SupportDocumentId"]);
                    DataRow[] foundRows;
                    foundRows = _Table.Select("ID=" + SupportDocumentId + "");
                    foreach (DataRow row in foundRows)
                    {

                        DocumentsNames += Convert.ToString(row[1]) + "*"; // ViewLink + "/" + _p.DocumentName;
                    }


                    //SupportDocumentId
                    // DocumentsNames += Convert.ToString(item["FileRef"]) + "*";
                }
            }
            Partner.DocumentName = DocumentsNames;
            Partner.DocumentId = Documents;

            clientContext.Credentials = credentials;
            spList = clientContext.Web.Lists.GetByTitle("QualityPartnerEvaluation");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();
            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                   @"<View>          <Query>      <Where><Eq><FieldRef Name='QPPNumber' /><Value Type='Text'>" + ParterId + "</Value></Eq></Where>     "
                   + "</Query>    <FieldRef Name='ID' />  <FieldRef Name='Title' /> <FieldRef Name='Comments' /> <FieldRef Name='Question' /></ViewFields>                               </View>";
                ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();
                StringBuilder SiteValuatoinQuestion = new System.Text.StringBuilder();
                foreach (ListItem item in listItems)
                {
                    string _TempQuestion = Convert.ToString(item["Question"]);
                    SiteValuatoinQuestion.Append(_TempQuestion);
                    SiteValuatoinQuestion.Append("|");

                    Partner.SiteEvaluatoinComments = Convert.ToString(item["Comments"]);

                }
                if (SiteValuatoinQuestion.Length > 0)
                {
                    string _RemoveLastPipeDelimeter = SiteValuatoinQuestion.ToString().Remove(SiteValuatoinQuestion.ToString().LastIndexOf('|')).ToString();
                    _RemoveLastPipeDelimeter = SiteValuatoinQuestion.ToString().Remove(SiteValuatoinQuestion.ToString().LastIndexOf('-')).ToString();
                    Partner.SiteEvalution = _RemoveLastPipeDelimeter;// SiteValuatoinQuestion.ToString();
                }
            }





            return Partner;

        }
        public QualityPartner GetDetailsInformatoinFinalStage(string ParterId)
        {

            List<QualityPartnerDocs> GetDocumentsStatusOb = new List<QualityPartnerDocs>();
            QualityPartner();
            // ParterId = "3";
            ArrayList _Array = new ArrayList();
            QualityPartner Partner = new QualityPartner();
            //string url = "http://sp-app2-dev:35897/VIS-DEV/";
            ClientContext clientContext = new ClientContext(url);
            NetworkCredential credentials = new NetworkCredential("bot1", "12345678", "ADQCC");
            DataTable _Table = new DataTable();
            //var ViewLink = "http://sp-app2-dev:35897/";

            clientContext.Credentials = credentials;
            Microsoft.SharePoint.Client.List spList = clientContext.Web.Lists.GetByTitle("QualityPartners");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();
            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                   @"<View>          <Query>      <Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + ParterId + "</Value></Eq></Where>           </Query>      <FieldRef Name='ID' />"
                   + "<FieldRef Name='SupportDocumentId' /></ViewFields>"
                    + "<FieldRef Name='TradeLiecense' /></ViewFields>"
                   + "<FieldRef Name='EntityFactoryName' /></ViewFields>"
                   + "<FieldRef Name='OwnerName' /></ViewFields>"
                   + "<FieldRef Name='TelephoneNumber' /></ViewFields>"
                   + "<FieldRef Name='RequestCatagory' /></ViewFields>"
                  + " </View>";
                ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();

                foreach (ListItem item in listItems)
                {
                    Partner.TradeLicense = Convert.ToString(item["TradeLiecense"]);
                    Partner.FactoryName = Convert.ToString(item["EntityFactoryName"]);
                    Partner.OwnerName = Convert.ToString(item["OwnerName"]);
                    Partner.TelephoneNumber = Convert.ToString(item["TelephoneNumber"]);
                    Partner.RequestCatagory = Convert.ToString(item["RequestCatagory"]);

                }
            }


            //</Neq><FieldRef Name='SupportDocumentId' /><Value Type='Text'>FinalApproval</Value></Neq>

            clientContext.Credentials = credentials;
            spList = clientContext.Web.Lists.GetByTitle("QualityPartnerSupportingDocs");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();
            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                     @"<View>          <Query>      <Where><Gt><FieldRef Name='ID' /><Value Type='Counter'>0</Value></Gt></Where>           </Query>    <FieldRef Name='ID' />  <FieldRef Name='Title' /></ViewFields>                               </View>";
                ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();

                _Table.Columns.Add("ID");
                _Table.Columns.Add("Title");

                foreach (ListItem item in listItems)
                {
                    //string UploadedDocumentID = Convert.ToString(item["ID"]);
                    // _Array.Add(UploadedDocumentID);
                    DataRow _ravi = _Table.NewRow();
                    _ravi["ID"] = Convert.ToInt32(item["ID"]);
                    _ravi["Title"] = Convert.ToString(item["Title"]); ;
                    _Table.Rows.Add(_ravi);

                }
            }
            string Documents = "";
            string DocumentsNames = "";

            clientContext.Credentials = credentials;
            spList = clientContext.Web.Lists.GetByTitle("QualityPartnerDocuments");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();
            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                   @"<View>          <Query>      <Where><Eq><FieldRef Name='PartnerID' /><Value Type='Text'>" + ParterId + "</Value></Eq></Where>           </Query>    <FieldRef Name='SupportDocumentId' /> <FieldRef Name='ID' />  <FieldRef Name='FileRef' /></ViewFields>                               </View>";
                ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();

                foreach (ListItem item in listItems)
                {
                    Documents += Convert.ToString(item["FileRef"]) + "*";
                    string SupportDocumentId = Convert.ToString(item["SupportDocumentId"]);
                    DataRow[] foundRows;
                    if (SupportDocumentId != "FinalApproval")
                    {
                        foundRows = _Table.Select("ID=" + SupportDocumentId + "");
                        foreach (DataRow row in foundRows)
                        {

                            DocumentsNames += Convert.ToString(row[1]) + "*"; // ViewLink + "/" + _p.DocumentName;
                        }
                    }
                    else
                    {
                        DocumentsNames += "Additional Documents" + "*";
                    }


                    //SupportDocumentId
                    // DocumentsNames += Convert.ToString(item["FileRef"]) + "*";
                }
            }
            Partner.DocumentName = DocumentsNames;
            Partner.DocumentId = Documents;

            clientContext.Credentials = credentials;
            spList = clientContext.Web.Lists.GetByTitle("QualityPartnerEvaluation");
            clientContext.Load(spList);
            clientContext.ExecuteQuery();
            if (spList != null && spList.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                   @"<View>          <Query>      <Where><Eq><FieldRef Name='QPPNumber' /><Value Type='Text'>" + ParterId + "</Value></Eq></Where>     "
                   + "</Query>    <FieldRef Name='ID' />  <FieldRef Name='Title' /> <FieldRef Name='Comments' /> <FieldRef Name='Question' /></ViewFields>                               </View>";
                ListItemCollection listItems = spList.GetItems(camlQuery);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();
                StringBuilder SiteValuatoinQuestion = new System.Text.StringBuilder();
                foreach (ListItem item in listItems)
                {
                    string _TempQuestion = Convert.ToString(item["Question"]);
                    SiteValuatoinQuestion.Append(_TempQuestion);
                    SiteValuatoinQuestion.Append("|");

                    Partner.SiteEvaluatoinComments = Convert.ToString(item["Comments"]);

                }
                if (SiteValuatoinQuestion.Length > 0)
                {
                    string _RemoveLastPipeDelimeter = SiteValuatoinQuestion.ToString().Remove(SiteValuatoinQuestion.ToString().LastIndexOf('|')).ToString();
                    _RemoveLastPipeDelimeter = SiteValuatoinQuestion.ToString().Remove(SiteValuatoinQuestion.ToString().LastIndexOf('-')).ToString();
                    Partner.SiteEvalution = _RemoveLastPipeDelimeter;// SiteValuatoinQuestion.ToString();
                }
            }





            return Partner;

        }
        #endregion
        #region LabelingModule
        public string GetStickers(string InspectorsName,string ParamServiceName)
        {
            ParamServiceName = "Qabannah";
            InspectorsName = @"adqcc\s.younus";
            string Result = string.Empty;
            string Secure = string.Empty;
            string Verfified = string.Empty;
            string QccTagNumber = string.Empty;
            string Reject = string.Empty;

            try
            {





                using (ClientContext ctx = new ClientContext("http://sp-app2-dev:35897/VIS-DEV"))
                {
                    Web web = ctx.Web;
                    
                    List list = web.Lists.GetById(new Guid("a8eae500-c0d9-4fd1-8857-08deb1945eb3"));
                    var q = new CamlQuery() { ViewXml = "<View><Query><Where><And><Eq><FieldRef Name='ServiceName' /><Value Type='Text'>Scale</Value></Eq><And><Eq><FieldRef Name='Utilized' /><Value Type='Text'>No</Value></Eq><Eq><FieldRef Name='AssignedTo' /><Value Type='Text'>adqcc\\a.farooqi</Value></Eq></And></And></Where></Query></View>" };
                    var r = list.GetItems(q);
                    ctx.Load(r);
                    ctx.ExecuteQuery(); 

                    
                    foreach (var item in r)
                    {
                        Result += "3";
                    }
                }





                   /*
                string url = "http://sp-app2-dev:35897/VIS-DEV/";
                ClientContext clientContext = new ClientContext(url);
                DataTable _Table = new DataTable();
                Microsoft.SharePoint.Client.List spList = clientContext.Web.Lists.GetByTitle("Labels");
                clientContext.Load(spList);
                clientContext.ExecuteQuery();
                if (spList != null && spList.ItemCount > 0)
               {
                    Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml =
                        //@"<View>          <Query>      <Where><And><And><Eq><FieldRef Name='AssignedTo' /><Value Type='Text'>" + InspectorsName + "</Value></Eq>"
                        // + "<Eq><FieldRef Name='Utilized' /><Value Type='Text'>NO</Value></Eq></And><Eq><FieldRef Name='ServiceName' /><Value Type='Text'>" + ParamServiceName + "</Value></Eq></And>"
                        // + "</Where>      </Query> "    
                        //+ " </View>";


                   camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='ServiceName' /><Value Type='Text'>Scale</Value></Eq></Where></Query></View>";

                  

                 
                    ListItemCollection listItems = spList.GetItems(camlQuery);
                    clientContext.Load(listItems);
                    clientContext.ExecuteQuery();

                    foreach (ListItem item in listItems)
                    {
                        string StickerType = Convert.ToString(item["StickerType"]);
                        string ServicesName = Convert.ToString(item["ServiceName"]);
                        if (ServicesName == ParamServiceName)
                        {
                            string GeneratedIDTemp = Convert.ToString(item["GeneratedID"]); 
                            switch (StickerType)
                            {
                                case "Reject":
                                    Reject += GeneratedIDTemp + ",";
                                    break;


                                case "Secure":
                                    Secure += GeneratedIDTemp + ",";
                                    break;



                                case "Verify":
                                    Verfified += GeneratedIDTemp + ",";
                                    break;

                                case "QCCTagNumber":
                                    QccTagNumber += GeneratedIDTemp + ",";
                                    break;


                            }
                        }
                      //  Secure += Convert.ToString(item[""])+",";
                        Result = Verfified + "-" + Secure + "-" + Reject + "-" + QccTagNumber;



                    }
                }
                */
                

            }
            catch (Exception ex)
            {

                throw;
            }
            return Result;
        }




        #endregion
    }
}
 