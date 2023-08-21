using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using UnityEngine;

public class ErrorCardView : MonoBehaviour {
    [SerializeField] private ErrorCard _errorCardPrefab;
    [SerializeField] private Transform _errorCardParent;

    private SwitchBoxesManager _switchBoxesManager;
    private EventBus _eventBus;
    private List<ConnectionData> _errorsList;
    private List<ErrorCard> _errorCards;

    public void Init(List<ConnectionData> errorsList, EventBus eventBus) {
        _errorsList = errorsList;
        _eventBus = eventBus;
        _switchBoxesManager = ServiceLocator.Current.Get<SwitchBoxesManager>();
        _errorCards = new List<ErrorCard>();
        
        if (errorsList.Count > 0) {
            CreateErrorCards();
        } else {
            gameObject.SetActive(false);
        }
    }

    private void CreateErrorCards() {
        int index = 0;
        foreach (ConnectionData iEror in _errorsList) {
            Companent iConpanent = _switchBoxesManager.GetCompanentByName(iEror.CompanentName); // Компанент с ошибкой                                                                               
            Contact iContact = iConpanent.Contacts.Find(x => x.ContactType == iEror.ContactType); // Контакт с ошибкой

            ErrorCard newCard = Instantiate(_errorCardPrefab);
            newCard.Init(iContact, index);
            newCard.transform.parent = _errorCardParent.transform;
            newCard.OnErrorCardChanged += ShowErrorContact;

            _errorCards.Add(newCard);
            index++;
        }
    }

    private void ShowErrorContact(ErrorCard card) {
        foreach (var iCard in _errorCards) {
            iCard.Button.Status = false;
        }
        card.Button.Status = true;

        SwitchBox swichBox = card.Contact.GetParentCompanent().SwitchBox;
        _eventBus.Invoke(new ActiveSwitchBoxChangedSignal(swichBox));
        card.Contact.StartBlink();
    }
}
