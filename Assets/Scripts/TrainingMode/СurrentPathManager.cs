using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class СurrentPathManager : MonoBehaviour {
    private SwitchBoxesManager _switchBoxesManager;
    private SwitchBox _acticeSwitchBox;
    private EventBus _eventBus;

    private List<Companent> _sources;
    private List<Companent> _receivers;
    private List<Wire> _receiversWires;
    private List<WagoContact> _receiverWagoClipContacts;
    private List<Contact> _receiversCompanentsContacts;
    private List<ElectricFieldMovingView> _electricFieldMovingViews;
    private List<ElectricFieldMovingView> _electricFieldMovingViews_Line;
    private List<ElectricFieldMovingView> _electricFieldMovingViews_Neutral;
    private List<ElectricFieldMovingView> _electricFieldMovingViews_GroundConductor;

    private bool _pathEnd;

    public void Init (SwitchBoxesManager switchBoxesManager, EventBus eventBus) {
        _switchBoxesManager = switchBoxesManager;
        _eventBus = eventBus;
        CreateСurrentPath();
        //_eventBus.Subscribe<>
    }


    public void CreateСurrentPath() {
        bool pathEnd = false;
        _sources = new List<Companent>();
        _receivers = new List<Companent>();
        _receiversWires = new List<Wire>();
        _receiverWagoClipContacts = new List<WagoContact>();
        _receiversCompanentsContacts = new List<Contact>();
        _electricFieldMovingViews = new List<ElectricFieldMovingView>();
        _electricFieldMovingViews_Line = new List<ElectricFieldMovingView>();

        // Создаём путь от источника до всех приёмников по фазному проводнику
        _sources.Add(CreateStartPointCurrentPath(CompanentType.Input, ContactType.Line));         // Добавляем стартовую точку в массив источников
        for (int stepNumber = 1; stepNumber < 10; stepNumber++) {
            if (pathEnd == false) {
                foreach (Companent iSource in _sources) {    // Проверяем все точки, через которые проходит ток
                    Contact startSourcePoint = null;
                    WagoClip resiver = null;

                    if (iSource.Type == CompanentType.Input) {
                        startSourcePoint = iSource.GetContactByType(ContactType.Line);
                        resiver = FindReseiversWagoClip(startSourcePoint, iSource);
                        
                        if (resiver != null) {
                            AddReseiversElementToPath(resiver);
                        }
                    } else if (iSource.Type == CompanentType.Selector) {
                        List<ContactType> selectorContactsTypes = new List<ContactType>() { ContactType.Line, ContactType.Closed, ContactType.Open };
                        foreach (var item in selectorContactsTypes) {
                            startSourcePoint = iSource.GetContactByType(item);
                            resiver = FindReseiversWagoClip(startSourcePoint, iSource);

                            if (resiver != null) {
                                AddReseiversElementToPath(resiver);
                            }
                        }

                        _receiversCompanentsContacts.Clear();
                        _receiverWagoClipContacts.Clear();
                        _receiversWires.Clear();

                    } else if (iSource.Type == CompanentType.Output) {
                        continue;
                    }
                }

                _sources.Clear();
                if (_receivers.Count > 0) {
                    _sources.AddRange(_receivers);
                    _receivers.Clear();
                } else {
                    pathEnd = true;
                }            
            }
        }

        _electricFieldMovingViews_Line = _electricFieldMovingViews.Distinct().ToList();

        pathEnd = false;
        _sources.Clear();
        _receivers.Clear();
        _receiversWires.Clear();
        _receiverWagoClipContacts.Clear();
        _receiversCompanentsContacts.Clear();
        _electricFieldMovingViews.Clear();

        // Создаём путь от приёмников к источнику по нулевому проводу
        _sources.Add(CreateStartPointCurrentPath(CompanentType.Input, ContactType.Neutral));         // Добавляем стартовую точку в массив источников
        for (int stepNumber = 1; stepNumber < 10; stepNumber++) {
            if (pathEnd == false) {
                foreach (Companent iSource in _sources) {    // Проверяем все точки, через которые проходит ток
                    Contact startSourcePoint = null;
                    WagoClip resiver = null;

                    if (iSource.Type == CompanentType.Input) {
                        startSourcePoint = iSource.GetContactByType(ContactType.Neutral);
                        resiver = FindReseiversWagoClip(startSourcePoint, iSource);

                        if (resiver != null) {
                            AddReseiversElementToPath(resiver);
                        }
                    }
                    else if (iSource.Type == CompanentType.Output) {
                        continue;
                    }
                }

                _sources.Clear();
                if (_receivers.Count > 0) {
                    _sources.AddRange(_receivers);
                    _receivers.Clear();
                }
                else {
                    pathEnd = true;
                }
            }
        }

        _electricFieldMovingViews_Neutral = _electricFieldMovingViews.Distinct().ToList();

        pathEnd = false;
        _sources.Clear();
        _receivers.Clear();
        _receiversWires.Clear();
        _receiverWagoClipContacts.Clear();
        _receiversCompanentsContacts.Clear();
        _electricFieldMovingViews.Clear();

        // Создаём путь от приёмников к источнику по заземляющему проводу
        _sources.Add(CreateStartPointCurrentPath(CompanentType.Input, ContactType.GroundConductor));         // Добавляем стартовую точку в массив источников
        for (int stepNumber = 1; stepNumber < 10; stepNumber++) {
            if (pathEnd == false) {
                foreach (Companent iSource in _sources) {    // Проверяем все точки, через которые проходит ток
                    Contact startSourcePoint = null;
                    WagoClip resiver = null;

                    if (iSource.Type == CompanentType.Input) {
                        startSourcePoint = iSource.GetContactByType(ContactType.GroundConductor);
                        resiver = FindReseiversWagoClip(startSourcePoint, iSource);

                        if (resiver != null) {
                            AddReseiversElementToPath(resiver);
                        }
                    }
                    else if (iSource.Type == CompanentType.Output) {
                        continue;
                    }
                }

                _sources.Clear();
                if (_receivers.Count > 0) {
                    _sources.AddRange(_receivers);
                    _receivers.Clear();
                }
                else {
                    pathEnd = true;
                }
            }
        }

        _electricFieldMovingViews_GroundConductor = _electricFieldMovingViews.Distinct().ToList();

        pathEnd = false;
        _sources.Clear();
        _receivers.Clear();
        _receiversWires.Clear();
        _receiverWagoClipContacts.Clear();
        _receiversCompanentsContacts.Clear();
        _electricFieldMovingViews.Clear();

    }

    /// <summary>
    /// Подключенный к контакту Wago-зажим
    /// </summary>
    /// <param name="startSourcePoint"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    private WagoClip FindReseiversWagoClip(Contact startSourcePoint, Companent source) {
        if (startSourcePoint.ConnectionWire != null) {
            WagoClip resiver = IdentifyReceiver(startSourcePoint);
            //Debug.Log($"Контакт {startSourcePoint}, WagoClip {resiver}");
            
            if (resiver != null) {
                AddElementToPath(source.GetElectricFieldMovingView(startSourcePoint));              // Контакт компанента
                AddElementToPath(startSourcePoint.ConnectionWire.ElectricFieldMovingView);          // Провод

                _receiverWagoClipContacts.Add(startSourcePoint.ConnectionWire.EndContact);          // Контакт по которому Wago-зажим подключен к источнику
                _receiverWagoClipContacts.Add(null);                                                // Общая шина Wago-зажима

                foreach (WagoContact iWagoContact in resiver.WagoContacts) {
                    if (iWagoContact.ConnectedContact != null) {
                        if (_receiverWagoClipContacts.Contains(iWagoContact) != true) {
                            if (iWagoContact.ConnectionWire.StartContact.ContactType == ContactType.Neutral || iWagoContact.ConnectionWire.StartContact.ContactType == ContactType.GroundConductor) {
                                iWagoContact.ConnectionWire.ElectricFieldMovingView.SwichDirection();
                            }
                            _receiverWagoClipContacts.Add(iWagoContact);
                            _receiversWires.Add(iWagoContact.ConnectionWire);
                        } 
                    }
                }

                List<Companent> reseiversCompanents = resiver.GetParentCompanents(ContactType.Line);
                reseiversCompanents.Remove(source);
                if (reseiversCompanents.Count > 0) {
                    _receivers.Clear();
                    foreach (var iCompanent in reseiversCompanents) {
                        _receivers.Add(iCompanent);
                        _receiversCompanentsContacts.Add(iCompanent.GetContactByType(startSourcePoint.ContactType));
                    }  
                }
            } 
            return resiver;
        }
        Debug.LogWarning($"Контакт {startSourcePoint} компанета {source.Name} не подключен!");
        return null;
    }

    /// <summary>
    /// Создание стартовой точки пути тока по схеме
    /// </summary>
    /// <param name="companent"></param>
    /// <param name="contact"></param>
    /// <returns></returns>
    public Companent CreateStartPointCurrentPath(CompanentType companent, ContactType contact) {
        _acticeSwitchBox = _switchBoxesManager.ActiveSwichBox;
        ConnectionData connectionData = new ConnectionData();
        connectionData.CompanentName = $"Input{_acticeSwitchBox.Number}";
        connectionData.CompanentType = companent;
        connectionData.ContactType = contact;

        Companent currentSource = _acticeSwitchBox.GetComponentByConnectionData(connectionData); 
        return currentSource;
    }

    /// <summary>
    /// Список подключенных к источнику элементов
    /// </summary>
    /// <param name="source"></param>
    /// <param name="contactType"></param>
    /// <returns></returns>
    public List<Companent> IdentifyReceivers(Companent source, ContactType contactType) { 
        Contact contact = source.GetContactByType(contactType);     /// Контакт через который осуществляется подключение
        List<Companent> receivers = contact.GetConnectionCompanents();
        receivers.Remove(source);
        
        return receivers;
    }

    public WagoClip IdentifyReceiver(Contact sourceContact) {
        WagoClip wago = sourceContact.ConnectionWire.EndContact.ParentWagoClip;
        wago.CheckFieldMovingDirection();
        return wago;
    }

    private void AddReseiversElementToPath(WagoClip resiver) {
        // Wago-контакты
        foreach (var iWagoContact in _receiverWagoClipContacts) {
            ElectricFieldMovingView efmv = iWagoContact != null ? iWagoContact.GetElectricFieldMovingView() : resiver.GetCommomBusElectricFieldMovingView();
            AddElementToPath(efmv);
        }

        // Линии от Wago-зажима к компанентам
        foreach (var iWire in _receiversWires) {
            AddElementToPath(iWire.ElectricFieldMovingView);
        }

        // Подключенные к Wago-зажиму компаненты
        for (int i = 0; i < _receivers.Count; i++) {
            Companent companent = _receivers[i];
            Contact contact = _receiversCompanentsContacts[i];
            AddElementToPath(companent.GetElectricFieldMovingView(contact));
        }
    }

    private void AddElementToPath(ElectricFieldMovingView efmv) {
        if (_electricFieldMovingViews.Contains(efmv) != true) {
            _electricFieldMovingViews.Add(efmv);                                                                       
        }
    }

    [ContextMenu("Demonstration")]
    public void Demonstration() {
        StartCoroutine(RunDemonstration());
    }

    private IEnumerator RunDemonstration() {
        if (_electricFieldMovingViews_Line.Count > 1) {
            foreach (var item in _electricFieldMovingViews_Line) {
                yield return StartCoroutine(SecondFunction(item)); // Ожидание выполнения второй функции
            }
        }

        if (_electricFieldMovingViews_Neutral.Count > 1) {
            for (int i = _electricFieldMovingViews_Neutral.Count-1; i >= 0; i--) {
                yield return StartCoroutine(SecondFunction(_electricFieldMovingViews_Neutral[i])); // Ожидание выполнения второй функции
            }
        }

        if (_electricFieldMovingViews_GroundConductor.Count > 1) {
            for (int i = _electricFieldMovingViews_GroundConductor.Count-1; i >= 0; i--) {
                yield return StartCoroutine(SecondFunction(_electricFieldMovingViews_GroundConductor[i])); // Ожидание выполнения второй функции
            }
        }
    }

    private IEnumerator SecondFunction(ElectricFieldMovingView view) {
        yield return StartCoroutine(view.Demonstration());
    }
}
