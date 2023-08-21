using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class AttemptInfo {
    public string TaskID;
    public DateTime Date;
    public bool CheckResult;
    public float BuildTime;
    public ErrorInfo[] Errors;
    public SwichBoxInfo[] SwichBoxsInfo;

    public AttemptInfo() {

    }

    public AttemptInfo(string taskID, DateTime date, bool checkResult, float buildTime, ErrorInfo[] errors, SwichBoxInfo[] swichBoxsInfo) {
        TaskID = taskID;
        Date = date;
        CheckResult = checkResult;
        BuildTime = buildTime;
        Errors = errors;
        SwichBoxsInfo = swichBoxsInfo;
    }

    public AttemptInfo GetAttemptInfo(GeneralSwitchingResult generalSwitchingResult) {
        TaskID = generalSwitchingResult.TaskData.ID;
        Date = generalSwitchingResult.CurrentDate;
        CheckResult = generalSwitchingResult.CheckStatus;
        BuildTime = generalSwitchingResult.BuildingTime;

        if (!CheckResult) Errors = GetErrorsInfo(generalSwitchingResult.ErrorsList);
        else Errors = null;
           
        if (generalSwitchingResult.TaskData.Type == TaskType.Full) SwichBoxsInfo = GetSwichBoxsInfo(generalSwitchingResult.SingleSwichingResults);
        else SwichBoxsInfo = null;
        
        return this;
    }

    private ErrorInfo[] GetErrorsInfo(List<ConnectionData> data) {
        if (data.Count == 0) return null;

        List<ErrorInfo> errors = new List<ErrorInfo>();
        foreach (ConnectionData iError in data) {
            ErrorInfo error = new ErrorInfo();
            error.CompanentName = iError.CompanentName;
            error.CompanentType = iError.CompanentType.ToString();
            error.ContactType = iError.ContactType.ToString();

            errors.Add(error);
        }

        return errors.ToArray();
    }

    private SwichBoxInfo[] GetSwichBoxsInfo(List<SingleSwitchingResult> data) {
        if (data.Count == 0) return null;

        List<SwichBoxInfo> swichBoxsInfo = new List<SwichBoxInfo>();
        foreach (SingleSwitchingResult iResult in data) {
            SwichBoxInfo sbInfo = new SwichBoxInfo();
            sbInfo.Name = iResult.SwitchBoxNumer.ToString();
            sbInfo.CheckResult = iResult.CheckStatus;
            sbInfo.BuildTime = iResult.BuldingTime;
            sbInfo.Errors = GetErrorsInfo(iResult.ErrorList);

            swichBoxsInfo.Add(sbInfo);
        }

        return swichBoxsInfo.ToArray();
    }
}

    [Serializable]
    public class ErrorInfo {
        public string CompanentType;
        public string CompanentName;
        public string ContactType;

        public ErrorInfo() {

        }

        public ErrorInfo(string companentType, string companentName, string contactType) {
            CompanentType = companentType;
            CompanentName = companentName;
            ContactType = contactType;
        }

    }

    [Serializable]
    public class SwichBoxInfo {
        public string Name;
        public bool CheckResult;
        public float BuildTime;
        public ErrorInfo[] Errors;

        public SwichBoxInfo() {

        }

        public SwichBoxInfo(string name, bool checkResult, float buildTime, ErrorInfo[] errors) {
            Name = name;
            CheckResult = checkResult;
            BuildTime = buildTime;
            Errors = errors;
        }
    }

