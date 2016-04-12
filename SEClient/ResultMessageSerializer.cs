using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MT.pHLab.SE.V1;
using XMLSerializationCustomization;

namespace SERemoteLib
{
    public class SEResultMessageSerializer
    {

        public static string Serialize(ResultMessage objResultMessage)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(objResultMessage.GetType());
            StringWriter xmlStream = new StringWriter();
            xmlSerializer.Serialize(xmlStream, objResultMessage);
            return xmlStream.ToString();
        }

        public static ResultMessage Deserialize(string xmlResultMessage)
        {
            StringReader stringReader = new StringReader(xmlResultMessage);
            XmlTextReader reader = new XMLSerializationCustomization.PdkXmlTextReader(stringReader);

            ResultMessage objResultMessage = new ResultMessage();

            XmlRootAttribute xmlRoot = new XmlRootAttribute();
            xmlRoot.Namespace = "tf";
            //xmlRoot.ElementName = "";
            xmlRoot.IsNullable = true;
            XmlSerializer xmlser = new XmlSerializer(objResultMessage.GetType(), xmlRoot);

            return (ResultMessage)xmlser.Deserialize(reader);
        }

        public static string SerializeString(ResultMessage rm)
        {
            string sResultMessage = "";
            string sResult = "";
            string sResultType = "";
            string sResultUnit = "";

            try
            {
                Object Result = rm.result.Item;
                Type ResultType = Result.GetType();
                string[] a = ResultType.ToString().Split('.');
                sResultType = a[a.Length - 1];

                if (ResultType == typeof(DecimalResult))
                {
                    sResult = SerializeDecimalResult((DecimalResult)Result);
                }
                else if (ResultType == typeof(IntegerResult))
                {
                    sResult = ((IntegerResult)Result).value.ToString();
                }
                else if (ResultType == typeof(StringResult))
                {
                    sResult = ((StringResult)Result).value;
                }
                else if (ResultType == typeof(ResultPh))
                {

                    return GetText((ResultPh)Result);
                }
                else if (ResultType == typeof(ResultCnd))
                {
                    return GetText((ResultCnd)Result);
                }
                //else if (ResultType == typeof(ErrorResult))
                //{
                //    sResult = SerializeErrorResult((ErrorResult)Result);
                //}
                else
                {
                    sResult = string.Format("??? type: {0}", sResultType);
                }

            }
            catch (Exception)
            {
                sResult = "ERROR";
            }

            //get result unit
            sResultUnit = rm.unitstring;
            if (String.IsNullOrEmpty(sResultUnit))
            {
                sResultUnit = ((ECommonUnit)rm.unit).ToString();
            }

            sResultMessage = string.Format("{0}.{1}.{2} ({3}) -> {4} [{5}] (Prec: {6})"
                    , rm.rackid
                    , rm.slotid
                    , rm.groupid
                    , sResultType
                    , sResult
                    , sResultUnit
                    , rm.precision
                    );

            return sResultMessage;
        }

        private static string SerializeDecimalResult(DecimalResult r)
        {
            switch (r.state)
            {
                case EDecimalResultState.eDecimalResultState_Valid:
                    return r.value.ToString();
                case EDecimalResultState.eDecimalResultState_NaN:
                    return "NaN";
                case EDecimalResultState.eDecimalResultState_INF:
                    return "INF";
                default:
                    throw new ArgumentOutOfRangeException("r.state");

            }
        }

        static public string GetText(EAnalysisStatus enumAnalysisStatus)
        {
            switch (enumAnalysisStatus)
            {
                case EAnalysisStatus.eAnalysisStatusid_AnalysisStatus_OK:
                    return "OK";
                case EAnalysisStatus.eAnalysisStatusid_AnalysisStatus_OKStar:
                    return "OK*";
                case EAnalysisStatus.eAnalysisStatusid_AnalysisStatus_Failed:
                    return "Failed";
                case EAnalysisStatus.eAnalysisStatusid_AnalysisStatus_Error:
                    return "Error";
                case EAnalysisStatus.eAnalysisStatusid_AnalysisStatus_CriticalError:
                    return "Critical Error";
                case EAnalysisStatus.eAnalysisStatusid_AnalysisStatus_Terminate:
                    return "Terminate";
                default:
                    return string.Empty;
            }
        }

        static public string GetText(EResultLimitState enumResultLimitState)
        {
            switch (enumResultLimitState)
            {
                case EResultLimitState.eResultLimit_NotChecked:
                    return "Not Checked";
                case EResultLimitState.eResultLimit_InRange:
                    return "In Range";
                case EResultLimitState.eResultLimit_OutUpper:
                    return "Error (above high limit)";
                case EResultLimitState.eResultLimit_OutLower:
                    return "Error (below low limit)";
                case EResultLimitState.eResultLimit_min:
                    return "Minimum";
                case EResultLimitState.eResultLimit_max:
                    return "Maximum";
                default:
                    return string.Empty;
            }
        }

        static public string GetText(ECommonUnit eCommonUnit)
        {
            switch (eCommonUnit)
            {
                case ECommonUnit.eCommonUnitid_CommonUnit_None:
                    return string.Empty;
                case ECommonUnit.eCommonUnitid_CommonUnit_PSU:
                    return "psu";
                case ECommonUnit.eCommonUnitid_CommonUnit_PPT:
                    return "ppt";
                case ECommonUnit.eCommonUnitid_CommonUnit_MMOLL:
                    return "mmol/L";
                case ECommonUnit.eCommonUnitid_CommonUnit_MOLL:
                    return "mol/L";
                case ECommonUnit.eCommonUnitid_CommonUnit_TDSPPT:
                    return "ppt(‰)";
                case ECommonUnit.eCommonUnitid_CommonUnit_MgL:
                    return "mg/L";
                case ECommonUnit.eCommonUnitid_CommonUnit_PPM:
                    return "ppm";
                case ECommonUnit.eCommonUnitid_CommonUnit_Percent:
                    return "%";
                case ECommonUnit.eCommonUnitid_CommonUnit_PX:
                    return "pX";
                case ECommonUnit.eCommonUnitid_CommonUnit_PH:
                    return "pH";
                case ECommonUnit.eCommonUnitid_CommonUnit_MV:
                    return "mV";
                case ECommonUnit.eCommonUnitid_CommonUnit_RelMV:
                    return "Rel.mV";
                case ECommonUnit.eCommonUnitid_CommonUnit_USCM:
                    return "µS/cm";
                case ECommonUnit.eCommonUnitid_CommonUnit_MSCM:
                    return "mS/cm";
                case ECommonUnit.eCommonUnitid_CommonUnit_SPerM:
                    return "S/m";
                case ECommonUnit.eCommonUnitid_CommonUnit_USPerM:
                    return "µS/m";
                case ECommonUnit.eCommonUnitid_CommonUnit_MSPerM:
                    return "mS/m";
                case ECommonUnit.eCommonUnitid_CommonUnit_GPerL:
                    return "g/L";
                case ECommonUnit.eCommonUnitid_CommonUnit_mBar:
                    return "mbar";
                case ECommonUnit.eCommonUnitid_CommonUnit_hPa:
                    return "hPa";
                case ECommonUnit.eCommonUnitid_CommonUnit_mmHg:
                    return "mmHg";
                case ECommonUnit.eCommonUnitid_CommonUnit_Atm:
                    return "Atm";
                case ECommonUnit.eCommonUnitid_CommonUnit_KOCM:
                    return "KΩ.cm";
                case ECommonUnit.eCommonUnitid_CommonUnit_MOCM:
                    return "MΩ.cm";
                case ECommonUnit.eCommonUnitid_CommonUnit_OCM:
                    return "Ω.cm";
                case ECommonUnit.eCommonUnitid_CommonUnit_GPerMOL:
                    return "g/Mol";
                case ECommonUnit.eCommonUnitid_CommonUnit_CellConstant:
                    return "CC";
                case ECommonUnit.eCommonUnitid_CommonUnit_MVPerPH:
                    return "mV/pH";
                case ECommonUnit.eCommonUnitid_CommonUnit_MVPerPX:
                    return "mV/pX";
                case ECommonUnit.eCommonUnitid_CommonUnit_ML:
                    return "mL";
                case ECommonUnit.eCommonUnitid_CommonUnit_Celsius:
                    return "°C";
                case ECommonUnit.eCommonUnitid_CommonUnit_Fahrenheit:
                    return "°F";
                case ECommonUnit.eCommonUnitid_CommonUnit_Ohm:
                    return "Ω";
                case ECommonUnit.eCommonUnitid_CommonUnit_Seconds:
                    return "s";
                case ECommonUnit.eCommonUnitid_CommonUnit_V:
                    return "V";
                case ECommonUnit.eCommonUnitid_CommonUnit_PercentPerCenti:
                    return "%";
                case ECommonUnit.eCommonUnitid_CommonUnit_KOhm:
                    return "K.Ω";
                case ECommonUnit.eCommonUnitid_CommonUnit_MOhm:
                    return "M.Ω";
                default:
                    return string.Empty;
            }
        }

        static public string GetText(EResultQuality enumResultQuality)
        {
            switch (enumResultQuality)
            {
                case EResultQuality.eDraftValue:
                    return "Draft";
                case EResultQuality.eDraftValue_Stable:
                    return "Draft/Stable";
                case EResultQuality.eManualEndpoint:
                    return "Manualendpoint";
                case EResultQuality.eAutoEndpoint_Strict:
                    return "Automatic/Strict";
                case EResultQuality.eAutoEndpoint_Normal:
                    return "Automatic/Normal";
                case EResultQuality.eAutoEndpoint_Fast:
                    return "Automatic/Standard";
                case EResultQuality.eAutoEndpoint_UserDef:
                    return "Automatic/User defined";
                case EResultQuality.eTimedEndpoint:
                    return "Timed";
                default:
                    return "Unspecified";
            }
        }

        static public string GetText(ResultCnd result)
        {
            string output = string.Format("Cnd: {0}{1}, {2}{3} ({4} {5})",
            result.resultValue.Value,
            GetText(result.resultUnit),
            result.rawTemperature.Value,
            GetText(result.temperatureUnit),
            GetText(result.resultQuality),
            GetText(result.resultLimitState));

            return output;
        }
        static public string GetText(ResultPh result)
        {
            string output = string.Format("Ph: {0}{1}, {2}{3} ({4} {5})",
            result.resultValue.Value,
            GetText(result.resultUnit),
            result.rawTemperature.Value,
            GetText(result.temperatureUnit),
            GetText(result.resultQuality),
            GetText(result.resultLimitState));

            return output;
        }
    }
}
