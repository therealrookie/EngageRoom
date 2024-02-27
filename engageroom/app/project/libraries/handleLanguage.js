let loadedSubId = CrComLib.subscribeState("o", "ch5-import-htmlsnippet:page1-import-page", (value) => {
  if (value["loaded"]) {
    onInit();
    setTimeout(() => {
      CrComLib.unsubscribeState("o", "ch5-import-htmlsnippet:page1-import-page", loadedSubId);
      loadedSubId = "";
    });
  }
});

function onInit() {
  let lang;

  const languagePack = {
    en: {
      welcomeMsg: "WELCOME TO PARIS",
      welcomeHint: "Start the system with this button.",
      btnStart: "START",
      txtToday: "Date",
      txtTime: "Time",

      localUseHeader: "Local Use",
      meetingHeader: "Meeting",
      zoomHeader: "Zoom",

      controlHeader: "Control",
      controlHint: "Click to control the device",
      ledControlButton: "LED Control",
      monitorControlButton: "Monitor Control",
      cameraControlButton: "Camera Control",

      sourceHeader: "Source",
      sourceHint: "Choose a video source",
      tvPlayerButton: "TV Player",
      signageButton: "adiTV",
      laptopInputButton: "Laptop Input",
      videoConButton: "Video Conference",

      volumeHeader: "Volume",
      volumeHint: "Change volume",
      roomSoundVolume: "Room Sound Volume",
      ledButtonsHeader: "LEDs",
      ledButtonsHint: "Turn LEDs on or off",
      monitorButtonsHeader: "Monitor",
      monitorButtonsHint: "Turn Monitor on or off",

      brandMusicHeader: "Brand Music",
      mediaLevelHeader: "Media Level",
      micLevelHeader: "Mic Level",

      cameraHeader: "Camera",
      cameraHint: "Select preset or move manually",

      preset01Button: "PRESET 1",
      preset02Button: "PRESET 2",
      preset03Button: "PRESET 3",
      previewButton: "PREVIEW",
      setButton: "SET",

      channelHeader: "Channels",

      homeButtonHeader: "Home",
      backButtonHeader: "Back",

      displayHeader: "Display",
      on: "ON",
      off: "OFF",
    },
    fr: {
      welcomeMsg: "BIENVENUE À PARIS",
      welcomeHint: "Démarrez le système avec ce bouton.",
      btnStart: "DÉMARRER",
      txtToday: "Date",
      txtTime: "Heure",

      localUseHeader: "Utilisation locale",
      meetingHeader: "Réunion",

      controlHeader: "Contrôle",
      controlHint: "Cliquez pour contrôler l'appareil",
      ledControlButton: "Contrôle LED",
      monitorControlButton: "Contrôle de l'écran",
      cameraControlButton: "Contrôle de la caméra",
      sourceHeader: "Source",
      sourceHint: "Choisissez une source vidéo",
      zoomHeader: "Zoom",
      tvPlayerButton: "Chaînes TV",
      signageButton: "adiTV",
      laptopInputButton: "Entrée Ordinateur portable",
      videoConButton: "Video Conference",
      volumeHeader: "Volume",
      volumeHint: "Changer le volume",
      roomSoundVolume: "Volume du son de la salle",
      ledButtonsHeader: "LEDs",
      ledButtonsHint: "Allumez ou éteignez les LEDs",
      monitorButtonsHeader: "Moniteur",
      monitorButtonsHint: "Allumez ou éteignez le moniteur",
      brandMusicHeader: "Musique de marque",
      mediaLevelHeader: "Niveau des médias",
      micLevelHeader: "Niveau du microphone",
      previewButton: "APERÇU",
      setButton: "CONFIGURER",
      cameraHeader: "Caméra",
      cameraHint: "Sélectionnez un préréglage ou déplacez manuellement",
      preset01Button: "PRÉRÉGLAGE 1",
      preset02Button: "PRÉRÉGLAGE 2",
      preset03Button: "PRÉRÉGLAGE 3",
      channelHeader: "Chaînes",
      homeButtonHeader: "Accueil",
      backButtonHeader: "Retour",
      displayHeader: "Affichage",
      on: "ON",
      off: "OFF",
    },
  };

  // CONSTANTS

  const btnEnglish = document.getElementById("chooseEnglish");
  const btnFrench = document.getElementById("chooseFrench");
  const startButton = document.getElementById("startButton");

  let codeIsCorrect = false;
  let nextPageNr;
  let inactivityTimeout;

  // -------------------------- FIRE ALARM CHECK -------------------------------------------------------------------

  let currPage;

  CrComLib.subscribeState("b", "controlPages.fireAlarmFb", (fire) => {
    fire ? switchPage(100) : switchPage(currPage);
  });

  // EVENT LISTENERS
  btnEnglish.addEventListener("click", function () {
    CrComLib.publishEvent("b", "selectLanguage.isEnglish", true);
  });

  btnFrench.addEventListener("click", function () {
    CrComLib.publishEvent("b", "selectLanguage.isEnglish", false);
  });

  startButton.addEventListener("click", function () {
    CrComLib.publishEvent("n", "controlPages.page", 2);
  });

  // ----------------------------------- CHANGE LANGUAGE ---------------------------------------

  CrComLib.subscribeState("b", "selectLanguage.isEnglishFb", (value) => {
    lang = value ? "en" : "fr";

    changeLangButtonAppearance(value);
    switchLanguage();
  });

  function switchLanguage() {
    const elements = document.querySelectorAll("[data-lang-key]");
    elements.forEach((element) => {
      const key = element.getAttribute("data-lang-key");
      element.textContent = languagePack[lang][key];
    });
  }

  btnEnglish.classList.add("buttonPressed");

  function changeLangButtonAppearance(isEnglish) {
    if (isEnglish) {
      btnEnglish.classList.add("buttonPressed");
      btnFrench.classList.remove("buttonPressed");
    } else {
      btnFrench.classList.add("buttonPressed");
      btnEnglish.classList.remove("buttonPressed");
    }
  }

  // ----------------------------------- INPUT CODE AND TIMEOUT ---------------------------------------

  function resetInactivityTimeout() {
    clearTimeout(inactivityTimeout);

    inactivityTimeout = setTimeout(() => {
      let page = nextPageNr == 6 ? 3 : nextPageNr;
      CrComLib.publishEvent("n", "controlPages.page", page);
      prevPageSecure = false;
    }, 20000);
  }

  document.addEventListener("touchstart", resetInactivityTimeout);

  CrComLib.subscribeState("s", "controlPages.codeInputFb", (inputCode) => {
    if (inputCode === "911") {
      codeIsCorrect = true;
      setTimeout(() => {
        switchPage(nextPageNr);
      }, 100);
    } else {
      codeIsCorrect = false;
    }
  });

  CrComLib.subscribeState("n", "controlPages.pageFb", (value) => {
    currPage = value;
    switchPage(value);

    value < 99 ? CrComLib.publishEvent("n", "controlPages.previousPage", value) : undefined;

    CrComLib.publishEvent("s", "controlPages.codeInput", "");

    setTimeout(() => {
      switchLanguage();
    }, 50);
  });

  function switchPage(pageNr) {
    let nextPageName;
    switch (pageNr) {
      case 1:
        nextPageName = "page1";
        templatePageModule.navigateTriggerViewByPageName(nextPageName);

        break;
      case 2:
        nextPageName = "selectlocality";
        templatePageModule.navigateTriggerViewByPageName(nextPageName);
        break;
      case 3:
        nextPageName = "localuse";
        templatePageModule.navigateTriggerViewByPageName(nextPageName);

        break;
      case 4:
        nextPageName = "ledcontrol";
        templatePageModule.navigateTriggerViewByPageName(nextPageName);

        break;
      case 5:
        nextPageName = "monitorcontrol";
        templatePageModule.navigateTriggerViewByPageName(nextPageName);

        break;
      case 6:
        nextPageName = codeIsCorrect ? "cameracontrol" : "keypadinput";
        templatePageModule.navigateTriggerViewByPageName(nextPageName);
        break;
      case 7:
        nextPageName = "meeting";
        templatePageModule.navigateTriggerViewByPageName(nextPageName);
        break;
      case 100:
        nextPageName = "fire";
        templatePageModule.navigateTriggerViewByPageName(nextPageName);
        break;
      case 101:
        nextPageName = "loading";
        templatePageModule.navigateTriggerViewByPageName(nextPageName);
        break;
      default:
        break;
    }

    nextPageNr = pageNr;

    setTimeout(() => {
      switchLanguage();
    }, 50);
  }
}
