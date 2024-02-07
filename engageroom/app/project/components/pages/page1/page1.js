/**
 * Copyright (C) 2023 to the present, Crestron Electronics, Inc.
 * All rights reserved.
 * No part of this software may be reproduced in any form, machine
 * or natural, without the express written consent of Crestron Electronics.
 * Use of this source code is subject to the terms of the Crestron Software License Agreement 
 * under which you licensed this source code.  
 *
 * This code was automatically generated by Crestron's code generation tool.
*/
/*jslint es6 */
/*global serviceModule, CrComLib */

const page1Module = (() => {
    'use strict';

    // BEGIN::CHANGEAREA - your javascript for page module code goes here         

    /**
     * Initialize Method
     */
    function onInit() {
        function updateDateTime() {
            var now = new Date();
    
            var date = now.getDate().toString().padStart(2, '0') + '/'
                     + (now.getMonth() + 1).toString().padStart(2, '0') + '/'
                     + now.getFullYear();
    
            var time = now.getHours().toString().padStart(2, '0') + ':'
                     + now.getMinutes().toString().padStart(2, '0');
    
            document.getElementById("currentDate").innerHTML = date;
            document.getElementById("currentTime").innerHTML = time;
        }
    
        setInterval(updateDateTime, 1000);
        updateDateTime();
    }

    /**
     * private method for page class initialization
     */
    let loadedSubId = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:page1-import-page', (value) => {
        if (value['loaded']) {
            onInit();
            setTimeout(() => {
                CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:page1-import-page', loadedSubId);
                loadedSubId = '';
            });
        }
    });

    /**
     * All public method and properties are exported here
     */
    return {
    };

    // END::CHANGEAREA

})();