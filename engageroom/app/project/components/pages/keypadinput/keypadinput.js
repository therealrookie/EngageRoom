const keypadinputModule = (() => {
    'use strict';

    function onInit() {
        const keypadInputPage = document.getElementById("keypadinput-page");
        const dummyInput = keypadInputPage.querySelector('#dummyInput');

        const homeButton = keypadInputPage.querySelector("#homeButtonColumn");
        const backButton = keypadInputPage.querySelector("#backButtonColumn");

        

        let inputString = '';

        CrComLib.subscribeState('s', 'controlPages.codeInputFb', (value) => {
            inputString = value;
            dummyInput.textContent = '•'.repeat(inputString.length);
        })

        keypadInputPage.querySelectorAll('.number').forEach(button => {
            button.addEventListener('touchstart', function () {
                if (inputString.length < 5) {
                    inputString += button.getAttribute('data-key');
                    CrComLib.publishEvent('s', 'controlPages.codeInput', inputString);
                }
            });

        });

        keypadInputPage.querySelector('.deleteButton').addEventListener('touchstart', function () {
            inputString = inputString.slice(0, -1);

            CrComLib.publishEvent('s', 'controlPages.codeInput', inputString);
        });


        // --------- LISTEN ON HOME BUTTON -----------------------------------------
        homeButton.addEventListener('click', function () {
                CrComLib.publishEvent('n', 'controlPages.page', 1);
        });

        // --------- LISTEN ON BACK BUTTON -----------------------------------------
        backButton.addEventListener('click', function () {
                CrComLib.publishEvent('n', 'controlPages.page', 2);
        });
    }

    let loadedSubId = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:keypadinput-import-page', (value) => {
        if (value['loaded']) {
            onInit();
            setTimeout(() => {
                CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:keypadinput-import-page', loadedSubId);
                loadedSubId = '';
            });
        }
    }); 

    return {
    };

})();