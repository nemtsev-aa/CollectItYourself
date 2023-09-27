using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� ��������� � ������ "��������"
/// </summary>
public class LearningModeProgressManager : ProgressManager {
    private IEnumerable<ChapterDescription> _chaptersList;

    public override void Init() {
        base.Init();

        _chaptersList = ServiceLocator.Current.Get<ChapterController>().Chapters;
    }

    //public void Finished(ChapterFinishedSignal signal) {
    //    if (signal.GeneralSwitchingResult.CheckStatus && signal.GeneralSwitchingResult.TaskData.TaskStatus == TaskStatus.Unlock) {
    //        _currentExpValue += signal.GeneralSwitchingResult.TaskData.ExpValue;
    //    }
    //    _eventBus.Invoke(new TrainingProgressChangedSignal(_currentExpValue, _fullExpAmount));
    //}

    //public override void ShowProgressValue(TaskListCreatedSignal signal) {
    //    GetCurrentProgressValue();
    //}

    ///// <summary>
    ///// ��������� �������� ���������� ��������� 
    ///// </summary>
    //public void GetCurrentProgressValue() {
    //    GetCurrentExpValue();
    //    GetFillExpAmount();

    //    _eventBus.Invoke(new TrainingProgressChangedSignal(_currentExpValue, _fullExpAmount));
    //}

    ///// <summary>
    ///// ������������ �������� �������� ���������
    ///// </summary>
    //public void ShowCurrentProgressValue() {
    //    _eventBus.Invoke(new TrainingProgressChangedSignal(_currentExpValue, _fullExpAmount));
    //}

    ///// <summary>
    ///// ��������� ���������� ������������� �����
    ///// </summary>
    //private void GetCurrentExpValue() {
    //    if (_chaptersList.Count() > 0) {
    //        foreach (var iTask in _chaptersList) {
    //            if (iTask.TaskStatus == TaskStatus.Complite) {
    //                _currentExpValue += iTask.ExpValue;
    //            }
    //        }
    //    }
    //}

    ///// <summary>
    ///// ���������� ���������� �����
    ///// </summary>
    //private void GetFillExpAmount() {
    //    if (_chaptersList.Count() > 0) {
    //        foreach (var iTask in _chaptersList) {
    //            _fullExpAmount += iTask.ExpValue;
    //        }
    //    }
    //}

    //public override void Dispose() {
    //    _eventBus.Unsubscribe<TaskListCreatedSignal>(ShowProgressValue);
    //}
}
