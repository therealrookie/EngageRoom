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

    // SEND VOLUME HAS CHANGED EVENTS
    CrComLib.subscribeState("n", "meetingControl.roomSoundVolumeFb", (value) => {
      if (value >= 0 && value <= 65535) {
        CrComLib.publishEvent("b", "meetingControl.volumeChangedRoomSoundVolume", true);
        CrComLib.publishEvent("b", "meetingControl.volumeChangedRoomSoundVolume", false);
      }
    });

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
    });

    // LISTEN ON BACK BUTTON
    backButton.addEventListener("click", function () {
      CrComLib.publishEvent("n", "controlPages.page", 2);
    });
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
