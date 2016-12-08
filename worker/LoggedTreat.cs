using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace worker
{
    class LoggedTreat
    {
        private string path, id, planUID, planName, beamName;
        private int beamNumber;
        private DataTable records;
        private DateTime logTime;

        public LoggedTreat(string path)
        {
            this.path = path;
            id = verifyId(Path.GetFileNameWithoutExtension(path).Split('_').First().ToString());
            string logTimeString = Path.GetFileNameWithoutExtension(Path.GetFileName(path).Split('_').Last().ToString());
            logTime = DateTime.ParseExact(logTimeString, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
        }

        public void getRecords()
        {
            int delay = 5;
            DateTime before = logTime.AddMinutes(-delay);
            DateTime after = logTime.AddMinutes(delay);
            this.records = SqlInterface.Query("select TreatmentRecord.PlanUID, Patient.PatientId, PlanSetup.PlanSetupId, RadiationHstry.RadiationId, RadiationHstry.RadiationNumber from TreatmentRecord, Patient, RTPlan, PlanSetup, RadiationHstry where TreatmentRecord.TreatmentRecordDateTime > '" + before.ToString("yyyy-MM-dd HH:mm:ss") + "' and TreatmentRecord.TreatmentRecordDateTime < '" + after.ToString("yyyy-MM-dd HH:mm:ss") + "' and TreatmentRecord.PatientSer=Patient.PatientSer and TreatmentRecord.RTPlanSer=RTPlan.RTPlanSer and PlanSetup.PlanSetupSer=RTPlan.PlanSetupSer and Patient.PatientId = '" + Id + "' and RadiationHstry.TreatmentRecordSer=TreatmentRecord.TreatmentRecordSer");
            selectRecord();
        }

        public DataTable Records
        {

            get { return records; }
        }

        private string verifyId(string text)
        {
            DataTable idCheck = SqlInterface.Query("select PatientId from Patient where PatientId = '" + text + "'");
            return idCheck.Rows[0]["PatientId"].ToString();
        }

        private void selectRecord()
        {
            foreach (DataRow row in records.Rows)
            {
                if (this.path.IndexOf(row["PlanSetupId"].ToString() + "_" + row["RadiationId"].ToString()) > -1)
                {
                    this.planUID = row["PlanUID"].ToString();
                    this.planName = row["PlanSetupId"].ToString();
                    this.beamName = row["RadiationId"].ToString();
                    this.beamNumber = (int)row["RadiationNumber"];
                    return;
                }
            }
        }

        public string Id
        {
            get { return id; }
        }

        public void writeToFile(string fileEnding)
        {
            string outFile = this.path + fileEnding;
            string[] writeLines = { "Patient ID:\t" + this.Id, "Plan Name:\t" + this.planName, "Plan UID:\t" + this.planUID, "Beam Number:\t" + this.beamNumber, "Beam Name:\t" + this.beamName };
            File.WriteAllLines(outFile, writeLines);
        }
    }
}
