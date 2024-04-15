const localuseModule = (() => {
  "use strict";

  function onInit() {
    const localUsePage = document.getElementById("localuse-page");
    const ledControl = localUsePage.querySelector("#ledControlButton");
    const monitorControl = localUsePage.querySelector("#monitorControlButton");
    const cameraControl = localUsePage.querySelector("#cameraControlButton");
    const homeButton = localUsePage.querySelector("#homeButtonColumn");
    const backButton = localUsePage.querySelector("#backButtonColumn");

    // --------- SEND VOLUME HAS CHANGED EVENTS -----------------------------------------

    const volumeDelay = 50;

    // General-purpose debounce function
    function debounce(func, wait) {
      let timeout;
      return function executedFunction(...args) {
        const later = () => {
          clearTimeout(timeout);
          func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
      };
    }

    // Function to handle volume changes
    function handleVolumeChange(volumeType, value) {
      if (value >= 0 && value <= 65535) {
        CrComLib.publishEvent("b", `localUse.volumeChanged${volumeType}`, true);
        // Assuming you need to reset the volumeChanged state immediately after setting it to true
        setTimeout(() => CrComLib.publishEvent("b", `localUse.volumeChanged${volumeType}`, false), 0);
      }
    }

    // Debounced handlers
    const brandMusicVolumeHandler = debounce(handleVolumeChange.bind(null, "BrandMusic"), volumeDelay);
    const micVolumeHandler = debounce(handleVolumeChange.bind(null, "MicVolume"), volumeDelay);
    const mediaLevelHandler = debounce(handleVolumeChange.bind(null, "MediaLevel"), volumeDelay);

    // Subscribe state changes
    CrComLib.subscribeState("n", "localUse.brandMusicVolumeFb", brandMusicVolumeHandler);
    CrComLib.subscribeState("n", "localUse.micVolumeFb", micVolumeHandler);
    CrComLib.subscribeState("n", "localUse.mediaLevelFb", mediaLevelHandler);

    // --------- SWITCH PAGES -----------------------------------------

    ledControl.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 4);
    });

    monitorControl.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 5);
    });

    cameraControl.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 6);
      camControlPageControl();
    });

    homeButton.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 1);
    });

    backButton.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 2);
    });

    function camControlPageControl() {
      CrComLib.publishEvent("b", "controlPages.camControlActivated", true);
      CrComLib.publishEvent("b", "controlPages.camControlActivated", false);
    }
  }

  let loadedSubId = CrComLib.subscribeState("o", "ch5-import-htmlsnippet:localuse-import-page", (value) => {
    if (value["loaded"]) {
      onInit();
      setTimeout(() => {
        CrComLib.unsubscribeState("o", "ch5-import-htmlsnippet:localuse-import-page", loadedSubId);
        loadedSubId = "";
      });
    }
  });

  return {};
})();
