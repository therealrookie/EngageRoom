const meetingModule = (() => {
  "use strict";

  function onInit() {
    // ----------------------------- HTML ELEMENTS --------------------------------------------------------------------------------------

    const meetingPage = document.getElementById("meeting-page");
    const homeButton = meetingPage.querySelector("#homeButtonColumn");
    const backButton = meetingPage.querySelector("#backButtonColumn");
    const zoomPlusButton = meetingPage.querySelector("#zoomPlus");
    const zoomMinusButton = meetingPage.querySelector("#zoomMinus");

    // ----------------------------- PRESET BUTTONS --------------------------------------------------------------------------------------

    let presetButtons = [
      {
        event: "meetingControl.preset01",
        feedback: "meetingControl.preset01Fb",
        valueTmp: false,
        value: false,
        id: "preset01Button",
      },
      {
        event: "meetingControl.preset02",
        feedback: "meetingControl.preset02Fb",
        valueTmp: false,
        value: false,
        id: "preset02Button",
      },
      {
        event: "meetingControl.preset03",
        feedback: "meetingControl.preset03Fb",
        valueTmp: false,
        value: false,
        id: "preset03Button",
      },
    ];

    // GET PRESET BUTTONS FEEDBACK
    presetButtons.forEach((button) => {
      CrComLib.subscribeState("b", button.feedback, (value) => {
        button.value = value;
        updateActivatedStyle(button);
      });
    });

    // LISTEN ON ALL PRESET BUTTONS
    presetButtons.forEach((button) => {
      const btnElement = meetingPage.querySelector(`#${button.id}`);
      btnElement.addEventListener("touchstart", () => {
        sendPressedPresetButton(button.id, true);
      });
      btnElement.addEventListener("touchend", () => {
        sendPressedPresetButton(button.id, false);
        setPresetButtonsToZero();
      });
      btnElement.addEventListener("touchcancel", () => {
        sendPressedPresetButton(button.id, false);
        setPresetButtonsToZero();
      });
    });

    // SEND CORRECT VALUES FOR EACH BUTTON
    function sendPressedPresetButton(buttonId, pressed) {
      presetButtons.forEach((button) => {
        const value = button.id === buttonId && pressed;
        CrComLib.publishEvent("b", button.event, value);
      });
    }

    function setPresetButtonsToZero() {
      presetButtons.forEach((button) => {
        CrComLib.publishEvent("b", button.event, false);
      });
    }

    // UPDATE STYLE OF PRESET BUTONS
    function updateActivatedStyle(button) {
      const element = meetingPage.querySelector("#" + button.id);
      button.value ? element.classList.add("buttonPressed") : element.classList.remove("buttonPressed");
    }

    // ----------------------------- VOLUME --------------------------------------------------------------------------------------

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
        CrComLib.publishEvent("b", `meetingControl.${volumeType}`, true);
        // Assuming you need to reset the volumeChanged state immediately after setting it to true
        setTimeout(() => CrComLib.publishEvent("b", `meetingControl.${volumeType}`, false), 0);
      }
    }

    // Debounced handlers
    const roomSoundVolumeHandler = debounce(handleVolumeChange.bind(null, "volumeChangedRoomSoundVolume"), volumeDelay);

    // Subscribe state changes
    CrComLib.subscribeState("n", "meetingControl.roomSoundVolumeFb", roomSoundVolumeHandler);

    // ----------------------------- ZOOM CONTROL --------------------------------------------------------------------------------------

    let zoomTimeout;
    let isZoomingIn = false;
    let isZoomingOut = false;

    // FEEDBACK IF ZOOMING IN
    CrComLib.subscribeState("b", "meetingControl.zoomPlusFb", (zoomPlus) => {
      if (zoomPlus) {
        zoomPlusButton.classList.add("buttonPressed");
      } else {
        zoomPlusButton.classList.remove("buttonPressed");
      }
    });

    // FEEDBACK IF ZOOMING OUT
    CrComLib.subscribeState("b", "meetingControl.zoomMinusFb", (zoomMinus) => {
      if (zoomMinus) {
        zoomMinusButton.classList.add("buttonPressed");
      } else {
        zoomMinusButton.classList.remove("buttonPressed");
      }
    });

    function zoomIn() {
      CrComLib.publishEvent("b", "meetingControl.zoomPlus", true);
    }

    // Function to handle zoom out
    function zoomOut() {
      CrComLib.publishEvent("b", "meetingControl.zoomMinus", true);
    }

    zoomPlusButton.addEventListener("touchstart", (event) => {
      event.preventDefault(); // Prevent default touch behavior (e.g., scrolling)
      if (!isZoomingOut) {
        isZoomingIn = true;
        zoomIn(); // Trigger zoomIn immediately
        // Start repeating action when the button is touched and held
        zoomTimeout = setInterval(zoomIn, 200); // Adjust the interval as needed
      }
    });

    // Add touchend event listener to stop zooming in
    zoomPlusButton.addEventListener("touchend", () => {
      isZoomingIn = false;
      CrComLib.publishEvent("b", "meetingControl.zoomPlus", false);
      clearInterval(zoomTimeout);
    });

    // Add touchstart event listener for zooming out
    zoomMinusButton.addEventListener("touchstart", (event) => {
      event.preventDefault(); // Prevent default touch behavior (e.g., scrolling)
      if (!isZoomingIn) {
        isZoomingOut = true;
        zoomOut(); // Trigger zoomOut immediately
        // Start repeating action when the button is touched and held
        zoomTimeout = setInterval(zoomOut, 200); // Adjust the interval as needed
      }
    });

    // Add touchend event listener to stop zooming out
    zoomMinusButton.addEventListener("touchend", () => {
      isZoomingOut = false;
      CrComLib.publishEvent("b", "meetingControl.zoomMinus", false);
      clearInterval(zoomTimeout);
    });

    // ----------------------------- NAVIGATION --------------------------------------------------------------------------------------

    // LISTEN ON HOME BUTTON
    homeButton.addEventListener("click", function () {
      CrComLib.publishEvent("n", "controlPages.page", 1);
      meetingPageControl();
    });

    // LISTEN ON BACK BUTTON
    backButton.addEventListener("click", function () {
      CrComLib.publishEvent("n", "controlPages.page", 2);
      meetingPageControl();
    });

    function meetingPageControl() {
      CrComLib.publishEvent("b", "controlPages.meetingDeactivated", true);
      CrComLib.publishEvent("b", "controlPages.meetingDeactivated", false);
    }
  }

  let loadedSubId = CrComLib.subscribeState("o", "ch5-import-htmlsnippet:meeting-import-page", (value) => {
    if (value["loaded"]) {
      onInit();
      setTimeout(() => {
        CrComLib.unsubscribeState("o", "ch5-import-htmlsnippet:meeting-import-page", loadedSubId);
        loadedSubId = "";
      });
    }
  });

  return {};
})();
