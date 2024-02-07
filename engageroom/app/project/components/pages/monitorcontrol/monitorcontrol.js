
const monitorcontrolModule = (() => {
    'use strict';

    function onInit() {

        // ----------------------------- HTML ELEMENTS ---------------------------------------------

        const monitorControlPage = document.getElementById("monitorcontrol-page");
        const homeButton = monitorControlPage.querySelector("#homeButtonColumn");
        const backButton = monitorControlPage.querySelector("#backButtonColumn");
        const buttonContainer = monitorControlPage.querySelector('#channelButtonContainer');
        const volumeColumn = monitorControlPage.querySelector("#volumeColumn");
        const channelColumn = monitorControlPage.querySelector("#channelColumn");
        const channelIcon = monitorControlPage.querySelector("#volumeColumn .toggle-icon");
        const volumeIcon = monitorControlPage.querySelector("#channelColumn .toggle-icon");
        const ledOnButton = monitorControlPage.querySelector('#ledOn');
        const ledOffButton = monitorControlPage.querySelector('#ledOff');


        // ----------------------------- SOURCE BUTTONS ---------------------------------------------
        let tvPlayerSelected = false;
        let currentColumn;
        let channelsAreAvailable;

        let sourceButtons = [
            { event: 'monitorControl.signage', feedback: 'monitorControl.signageFb', valueTmp: false, value: false, id: "signageButton" },
            { event: 'monitorControl.tvPlayer', feedback: 'monitorControl.tvPlayerFb', valueTmp: false, value: false, id: "tvPlayerButton" },
            { event: 'monitorControl.laptopInput', feedback: 'monitorControl.laptopInputFb', valueTmp: false, value: false, id: "laptopInputButton" },
            { event: 'monitorControl.videoCon', feedback: 'monitorControl.videoConFb', valueTmp: false, value: false, id: "videoConButton" }
        ]

        // GET SOURCE BUTTONS FEEDBACK
        sourceButtons.forEach(button => {
            CrComLib.subscribeState('b', button.feedback, (value) => {
                button.value = value;
                updateActivatedStyle(button);

                if (button.id === "tvPlayerButton" && value && channelsAreAvailable) {
                    tvPlayerSelected = true;
                } else if (value) {
                    tvPlayerSelected = false;
                } 

                updateShownColumn();

            });
        });


        // ACTIVATION / DEACTIVATION OF YEALINK BUTTON
        CrComLib.subscribeState('b', 'monitorControl.videoConPermittedFb', (isActive) => {
            if (!isActive) {
                document.getElementById(sourceButtons[3].id).classList.add('inactive');
            } else if (isActive) {
                document.getElementById(sourceButtons[3].id).classList.remove('inactive');
            }
        });

        // LISTEN ON ALL SOURCE BUTTONS
        sourceButtons.forEach(button => {
            const btnElement = monitorControlPage.querySelector(`#${button.id}`);
            btnElement.addEventListener("click", () => {
                sendPressedSourceButton(button.id);
            });
        });

        // SEND CORRECT VALUES FOR EACH BUTTON
        function sendPressedSourceButton(buttonId) {
            sourceButtons.forEach(button => {
                const value = button.id === buttonId;
                CrComLib.publishEvent('b', button.event, value);
            });
        }

        // UPDATE STYLE OF SOURCE BUTTONS
        function updateActivatedStyle(button) {
            const element = monitorControlPage.querySelector('#' +
                button.id);
            button.value ? element.classList.add('buttonPressed') : element.classList.remove('buttonPressed');
        }

        // ----------------------------- SWITCH BETWEEN VOLUME & CHANNELS --------------------------------------------------------------------------------------
        

        // MAKE CHANNEL-LIST VISIBLE
        CrComLib.subscribeState('b', 'monitorControl.showChannelsPageFb', (showChannel) => {
            currentColumn = showChannel ? "channel" : "volume";
            selectVisibleColumn(currentColumn);
        });

        // MAKE CHANNEL-LIST AVAILABLE / UNAVAILABLE
        CrComLib.subscribeState('b', 'monitorControl.channelsAvailableFb', (value) => {
            channelsAreAvailable = value;
        });

        //selectVisibleColumn("volume");

        // LISTEN ON VOLUME- OR CHANNEL-ICON
        volumeIcon.addEventListener("click", () => {
            CrComLib.publishEvent('b', 'monitorControl.showChannelsPage', false);
        });

        channelIcon.addEventListener("click", () => {
            CrComLib.publishEvent('b', 'monitorControl.showChannelsPage', true);

        });

        // CHANGE STYLE OF VOLUME OR CHANNEL COLUMN
        function selectVisibleColumn(column) {
            switch (column) {
                case "volume":
                    volumeColumn.classList.add("active");
                    channelColumn.classList.remove("active");
                    channelColumn.classList.add("inactive");
                    volumeColumn.classList.remove("inactive");
                    break;
                case "channel":
                    volumeColumn.classList.remove("active");
                    channelColumn.classList.add("active");
                    channelColumn.classList.remove("inactive");
                    volumeColumn.classList.add("inactive");
                    break;
            }
        }

        // SWITCH BETWEEN VOLUME OR CHANNEL COLUMN
        function updateShownColumn() {
            if (tvPlayerSelected) {
                selectVisibleColumn(currentColumn);
                channelIcon.style.visibility = "visible";
            } else {
                selectVisibleColumn("volume");
                channelIcon.style.visibility = "hidden";
            }
        }


        // ----------------------------- CHANNELS --------------------------------------------------------------------------------------

        // CREATE OBJECT WITH KEY: channelJoinFB, VALUE: ''
        const channelNames = Array.from({ length: 20 }, (_, index) => `monitorControl.channel${index + 1}Fb`);
        const channelInfo = {};

        channelNames.forEach(channel => {
            channelInfo[channel] = '';
        });


        // RECEIVE ALL CHANNEL NAMES
        for (const channelJoin in channelInfo) {
            CrComLib.subscribeState('s', channelJoin, (channelName) => {
                channelInfo[channelJoin] = channelName;
                updateChannelList();
            }
            );
        }

        // RECEIVE THE SELECTED CHANNEL
        CrComLib.subscribeState('s', 'monitorControl.selectedChannelFb', (channel) => {
            buttonContainer.querySelectorAll('.button').forEach(button => {
                if (button.textContent === channel) {
                    button.classList.add('buttonPressed');
                } else {
                    button.classList.remove('buttonPressed');
                }
            });
        });

        // UPDATE THE LIST OF SHOWN CHANNELS IN THE SENT ORDER
        function updateChannelList() {
            const sortedChannels = Object.entries(channelInfo)
                .map(([channelJoin, channelName]) => ({ channelJoin, channelName }))
                .filter(channel => channel.channelName.trim() !== '')
                .sort((a, b) => {
                    const numA = parseInt(a.channelJoin.match(/\d+/)[0]);
                    const numB = parseInt(b.channelJoin.match(/\d+/)[0]);
                    return numA - numB;
                });

            buttonContainer.innerHTML = '';

            sortedChannels.forEach(({ channelJoin, channelName }) => {
                const channelButton = document.createElement("div");
                channelButton.classList.add("button");
                channelButton.textContent = channelName;
                channelButton.setAttribute("data-channel-join", channelJoin);

                buttonContainer.appendChild(channelButton);

                channelButton.removeEventListener('click', handleChannelButtonClick);

                channelButton.addEventListener('click', handleChannelButtonClick);
            });
        }

        // SEND TEXT OF CLICKED CHANNEL BUTTON
        function handleChannelButtonClick(event) {
            const clickedButton = event.target;
            CrComLib.publishEvent('s', 'monitorControl.selectedChannel', clickedButton.textContent);
        }

        // ----------------------------- DISPLAY --------------------------------------------------------------------------------------

        // FEEDBACK IF DISPLAY IS ON
        CrComLib.subscribeState('b', 'monitorControl.ledOnFb', (displayOn) => {
            if (displayOn) {
                ledOnButton.classList.add('buttonPressed');
            } else {
                ledOnButton.classList.remove('buttonPressed');
            }
        });

        // FEEDBACK IF DISPLAY IS OFF
        CrComLib.subscribeState('b', 'monitorControl.ledOffFb', (displayOff) => {
            if (displayOff) {
                ledOffButton.classList.add('buttonPressed');
            } else {
                ledOffButton.classList.remove('buttonPressed');
            }
        });

        // LISTEN ON DISPLAY ON/OFF BUTTONS
        ledOnButton.addEventListener('touchstart', () => {
            CrComLib.publishEvent('b', 'monitorControl.ledOn', true);
            CrComLib.publishEvent('b', 'monitorControl.ledOff', false);
        });

        ledOffButton.addEventListener('touchstart', () => {
            CrComLib.publishEvent('b', 'monitorControl.ledOn', false);
            CrComLib.publishEvent('b', 'monitorControl.ledOff', true);
        });


        // ----------------------------- NAVIGATION --------------------------------------------------------------------------------------

        // LISTEN ON HOME BUTTON
        homeButton.addEventListener('click', function () {
            CrComLib.publishEvent('n', 'controlPages.page', 1);
        });

        // LISTEN ON BACK BUTTON
        backButton.addEventListener('click', function () {
            CrComLib.publishEvent('n', 'controlPages.page', 3);
        });

    }

    let loadedSubId = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:monitorcontrol-import-page', (value) => {
        if (value['loaded']) {
            onInit();
            setTimeout(() => {
                CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:monitorcontrol-import-page', loadedSubId);
                loadedSubId = '';
            });
        }
    });

    return {
    };

})();