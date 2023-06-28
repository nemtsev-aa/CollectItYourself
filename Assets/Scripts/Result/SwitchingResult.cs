using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwitchingResult {
    public string TaskName;
    public int SwitchBoxNumber;
    public List<SingleSwitchingResult> SingleSwichingResults = new List<SingleSwitchingResult>();

    private string ErrorsCountText;
    private string SwitchingTimesText;
    private float SwitchingTimesValue;
    private List<ConnectionData> ErrorsList = new List<ConnectionData>();

    public string GetErrorsCountText() {
        if (SingleSwichingResults.Count > 0) {
            int errorsCountValue = 0;
            int contactsCountValue = 0;

            foreach (SingleSwitchingResult iResult in SingleSwichingResults) {
                string[] iData = iResult.ErrorCountText.Split("/");
                int iErrorsCount = int.Parse(iData[0]);
                int iContactsCount = int.Parse(iData[1]);

                errorsCountValue += iErrorsCount;
                contactsCountValue += iContactsCount;
            }

            ErrorsCountText = errorsCountValue + "/" + contactsCountValue;
            return ErrorsCountText;
        }

        return null;
    }

    public float GetSwitchingTimesValue() {
        if (SingleSwichingResults.Count > 0) {
            float timesValue = 0f;
            foreach (SingleSwitchingResult iResult in SingleSwichingResults) {
                timesValue += iResult.SwitchingTimesValue;
            }
            return timesValue;
        }
        return 0f;
    }

    public string GetSwitchingTimesText() {
        if (SingleSwichingResults.Count > 0) {
            float timesValue = 0f;
            foreach (SingleSwitchingResult iResult in SingleSwichingResults) {
                timesValue += iResult.SwitchingTimesValue;
            }
            SwitchingTimesText = GetFormattedTime(timesValue);
            return SwitchingTimesText;
        }
        return null;
    }

    private string GetFormattedTime(float _timeValue) {
        int minutes = Mathf.FloorToInt(_timeValue / 60f);
        int seconds = Mathf.FloorToInt(_timeValue % 60f);
        int milliseconds = Mathf.FloorToInt((_timeValue * 1000f) % 1000f);

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public List<ConnectionData> GetErrorsList() {
        if (SingleSwichingResults.Count > 0) {
            foreach (SingleSwitchingResult iResult in SingleSwichingResults) {
                ErrorsList.AddRange(iResult.ErrorList);
            }
            return ErrorsList;
        }
        return null;
    }
}
