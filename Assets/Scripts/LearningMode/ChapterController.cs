using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterController : MonoBehaviour, IService {
    public IEnumerable<ChapterDescription> Chapters => _chapters;
    [SerializeField] private List<ChapterDescription> _chapters;

    public Description GetChapterDescription(ChapterType type) {
        return _chapters.FindLast(t => t.Type == type);
    }
}
