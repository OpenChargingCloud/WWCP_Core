"use strict";
/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
let topLeft = null;
var SearchResultsMode;
(function (SearchResultsMode) {
    SearchResultsMode[SearchResultsMode["listView"] = 0] = "listView";
    SearchResultsMode[SearchResultsMode["tableView"] = 1] = "tableView";
})(SearchResultsMode || (SearchResultsMode = {}));
//#region EncodeToken(AccessToken)
function EncodeToken(AccessToken) {
    const buffer = [];
    for (let i = AccessToken.length - 1; i >= 0; i--) {
        buffer.unshift(['&#', AccessToken.charCodeAt(i), ';'].join(''));
    }
    return buffer.join('');
}
//#endregion
// #region WWCPGet
function WWCPGet(RessourceURI, OnSuccess, OnError) {
    const ajax = new XMLHttpRequest();
    ajax.open("GET", RessourceURI, true);
    ajax.setRequestHeader("Accept", "application/json; charset=UTF-8");
    ajax.setRequestHeader("X-Portal", "true");
    const accessToken = localStorage.getItem("ocpiAccessToken");
    const accessTokenEncoding = localStorage.getItem("ocpiAccessTokenEncoding");
    if (accessToken)
        ajax.setRequestHeader("Authorization", "Token " + (accessTokenEncoding === "base64" ? btoa(accessToken) : accessToken));
    ajax.onreadystatechange = function () {
        if (this.readyState == 4) {
            if (this.status >= 100 && this.status < 300)
                OnSuccess?.(this.status, ajax.responseText, (key) => ajax.getResponseHeader(key));
            else
                OnError?.(this.status, ajax.responseText, (key) => ajax.getResponseHeader(key));
        }
    };
    ajax.send();
}
// #endregion
async function WWCPGetAsync(RessourceURI) {
    return new Promise((resolve, reject) => {
        const ajax = new XMLHttpRequest();
        ajax.open("GET", RessourceURI, true);
        ajax.setRequestHeader("Accept", "application/json; charset=UTF-8");
        ajax.setRequestHeader("X-Portal", "true");
        const accessToken = localStorage.getItem("ocpiAccessToken");
        const accessTokenEncoding = localStorage.getItem("ocpiAccessTokenEncoding");
        if (accessToken) {
            ajax.setRequestHeader("Authorization", "Token " + (accessTokenEncoding === "base64" ? btoa(accessToken) : accessToken));
        }
        ajax.onreadystatechange = function () {
            if (this.readyState === 4) {
                if (this.status >= 100 && this.status < 300) {
                    try {
                        const ocpiResponse = JSON.parse(ajax.responseText);
                        if (ocpiResponse.status_code >= 1000 &&
                            ocpiResponse.status_code < 2000) {
                            resolve([ocpiResponse, (key) => ajax.getResponseHeader(key)]);
                        }
                        else
                            reject(new Error(ocpiResponse.status_code + (ocpiResponse.status_message ? ": " + ocpiResponse.status_message : "")));
                    }
                    catch (exception) {
                        reject(new Error(exception instanceof Error ? exception.message : String(exception)));
                    }
                }
                else {
                    reject(new Error(`HTTP Status Code ${this.status}: ${ajax.responseText}`));
                }
            }
        };
        ajax.send();
    });
}
function WWCPStartSearch(requestURL, nameOfItem, idOfItem, nameOfItems, nameOfItems2, doListView, doTableView, linkPrefix, startView, context) {
    return WWCPStartSearch2(requestURL, () => "", () => { }, nameOfItem, idOfItem, nameOfItems, nameOfItems2, doListView, doTableView, linkPrefix, startView, context);
}
function WWCPStartSearch2(requestURL, searchFilters, doStartUp, nameOfItem, idOfItem, nameOfItems, nameOfItems2, doListView, doTableView, linkPrefix, startView, context) {
    requestURL = requestURL.indexOf('?') === -1
        ? requestURL + '?'
        : requestURL.endsWith('&')
            ? requestURL
            : requestURL + '&';
    let firstSearch = true;
    let offset = 0;
    let limit = 10;
    let currentDateFrom = null;
    let currentDateTo = null;
    let viewMode = startView !== null ? startView : SearchResultsMode.listView;
    const context__ = { Search: Search };
    let numberOfResults = 0;
    let linkURL = "";
    let filteredNumberOfResults = 0;
    let totalNumberOfResults = 0;
    const controlsDiv = document.getElementById("controls");
    const patternFilter = controlsDiv.querySelector("#patternFilterInput");
    const takeSelect = controlsDiv.querySelector("#takeSelect");
    const searchButton = controlsDiv.querySelector("#searchButton");
    const leftButton = controlsDiv.querySelector("#leftButton");
    const rightButton = controlsDiv.querySelector("#rightButton");
    const dateFilters = controlsDiv.querySelector("#dateFilters");
    const dateFrom = dateFilters?.querySelector("#dateFromText");
    const dateTo = dateFilters?.querySelector("#dateToText");
    const listViewButton = controlsDiv.querySelector("#listView");
    const tableViewButton = controlsDiv.querySelector("#tableView");
    const messageDiv = document.getElementById('message');
    const localSearchMessageDiv = document.getElementById('localSearchMessage');
    const searchResultsDiv = document.querySelector(".searchResults");
    const downLoadButton = document.getElementById("downLoadButton");
    function DoSearchError(Message) {
        messageDiv.innerHTML = Message;
        if (downLoadButton)
            downLoadButton.style.display = "none";
    }
    function Search(deletePreviousResults, resetSkip, whenDone) {
        if (resetSkip)
            offset = 0;
        if (patternFilter.value[0] === '#') {
            if (whenDone !== null)
                whenDone();
            return;
        }
        leftButton.disabled = true;
        rightButton.disabled = true;
        const filters = (patternFilter.value !== "" ? "&match=" + encodeURI(patternFilter.value) : "") +
            (searchFilters ? searchFilters() : "") +
            (currentDateFrom != null && currentDateFrom !== "" ? "&from=" + currentDateFrom : "") +
            (currentDateTo != null && currentDateTo !== "" ? "&to=" + currentDateTo : "");
        if (downLoadButton)
            downLoadButton.href = requestURL + "download" + filters;
        WWCPGet(requestURL + filters + "&offset=" + offset + "&limit=" + limit, (status, response, httpHeaders) => {
            try {
                if (status == 200 && response) {
                    const ocpiResponse = JSON.parse(response);
                    if (ocpiResponse.status_code >= 1000 &&
                        ocpiResponse.status_code < 2000) {
                        if (ocpiResponse?.data &&
                            Array.isArray(ocpiResponse.data)) {
                            const searchResults = ocpiResponse.data;
                            numberOfResults = searchResults.length;
                            linkURL = httpHeaders("Link") || "";
                            totalNumberOfResults = Number.parseInt(httpHeaders("X-Total-Count") || "0");
                            filteredNumberOfResults = Number.parseInt(httpHeaders("X-Filtered-Count") || "0");
                            if (Number.isNaN(totalNumberOfResults))
                                totalNumberOfResults = numberOfResults;
                            if (Number.isNaN(filteredNumberOfResults))
                                filteredNumberOfResults = totalNumberOfResults;
                            if (deletePreviousResults)
                                searchResultsDiv.innerHTML = "";
                            // ── Fixed: doStartUp is always a function (TS2774) ──
                            if (firstSearch) {
                                //doStartUp(JSONresponse);   // original call was commented out
                                firstSearch = false;
                            }
                            switch (viewMode) {
                                case SearchResultsMode.tableView:
                                    try {
                                        doTableView(searchResults, searchResultsDiv);
                                    }
                                    catch (exception) {
                                        console.debug("Exception in search table view: " + exception);
                                    }
                                    break;
                                case SearchResultsMode.listView:
                                    if (searchResults.length > 0) {
                                        let resultCounter = offset + 1;
                                        for (const searchResult of searchResults) {
                                            try {
                                                const searchResultAnchor = searchResultsDiv.appendChild(document.createElement('a'));
                                                searchResultAnchor.id = nameOfItem + "_" + idOfItem(searchResult);
                                                searchResultAnchor.className = "searchResult " + nameOfItem;
                                                if (linkPrefix) {
                                                    const prefix = linkPrefix(searchResult);
                                                    if (prefix !== null && prefix.length > 0)
                                                        searchResultAnchor.href = prefix + nameOfItems + "/" + idOfItem(searchResult);
                                                }
                                                doListView(resultCounter, searchResult, searchResultAnchor);
                                                resultCounter++;
                                            }
                                            catch (exception) {
                                                DoSearchError("Exception in search list view: " + exception);
                                            }
                                        }
                                        if (downLoadButton)
                                            downLoadButton.style.display = "block";
                                    }
                                    else {
                                        if (downLoadButton)
                                            downLoadButton.style.display = "none";
                                    }
                                    break;
                            }
                            messageDiv.innerHTML = searchResults.length > 0
                                ? "showing results " + (offset + 1) + " - " + (offset + Math.min(searchResults.length, limit)) +
                                    " of " + filteredNumberOfResults
                                : "no matching " + nameOfItems2 + " found";
                            if (offset > 0)
                                leftButton.disabled = false;
                            if (offset + limit < filteredNumberOfResults)
                                rightButton.disabled = false;
                        }
                        else
                            DoSearchError("Invalid search results!");
                    }
                    else
                        DoSearchError("WWCP Status Code " + ocpiResponse.status_code + (ocpiResponse.status_message ? ": " + ocpiResponse.status_message : ""));
                }
                else
                    DoSearchError("HTTP Status Code " + status + (response ? ": " + response : ""));
            }
            catch (exception) {
                DoSearchError("Exception occurred: " + exception);
            }
            if (whenDone)
                whenDone();
        }, (status, response, httpHeaders) => {
            DoSearchError("Server error: " + status + "<br />" + response);
            if (whenDone)
                whenDone();
        });
    }
    if (patternFilter !== null) {
        patternFilter.onchange = () => {
            if (patternFilter.value[0] !== '#') {
                offset = 0;
            }
        };
        patternFilter.onkeyup = (ev) => {
            if (patternFilter.value[0] !== '#') {
                if (ev.key === 'Enter')
                    Search(true);
            }
            else {
                const pattern = patternFilter.value.substring(1);
                const logLines = Array.from(document.getElementById('searchResults').getElementsByClassName('searchResult'));
                let numberOfMatches = 0;
                for (const logLine of logLines) {
                    if (logLine.innerHTML.indexOf(pattern) > -1) {
                        logLine.style.display = 'block';
                        numberOfMatches++;
                    }
                    else
                        logLine.style.display = 'none';
                }
                if (localSearchMessageDiv !== null) {
                    localSearchMessageDiv.innerHTML = numberOfMatches > 0
                        ? numberOfMatches + " local matches"
                        : "no matching " + nameOfItems2 + " found";
                }
            }
        };
    }
    limit = parseInt(takeSelect.options[takeSelect.selectedIndex].value);
    takeSelect.onchange = () => {
        limit = parseInt(takeSelect.options[takeSelect.selectedIndex].value);
        Search(true);
    };
    searchButton.onclick = () => { Search(true); };
    leftButton.onclick = () => {
        leftButton.classList.add("busy", "busyActive");
        rightButton.classList.add("busy");
        offset -= limit;
        if (offset < 0)
            offset = 0;
        Search(true, false, () => {
            leftButton.classList.remove("busy", "busyActive");
            rightButton.classList.remove("busy");
        });
    };
    rightButton.onclick = () => {
        leftButton.classList.add("busy");
        rightButton.classList.add("busy", "busyActive");
        offset += limit;
        Search(true, false, () => {
            leftButton.classList.remove("busy");
            rightButton.classList.remove("busy", "busyActive");
        });
    };
    document.onkeydown = (ev) => {
        if (ev.key === 'ArrowLeft' || ev.key === 'ArrowUp') {
            if (!leftButton.disabled)
                leftButton.click();
            return;
        }
        if (ev.key === 'ArrowRight' || ev.key === 'ArrowDown') {
            if (!rightButton.disabled)
                rightButton.click();
            return;
        }
    };
    if (listViewButton !== null) {
        listViewButton.onclick = () => {
            viewMode = SearchResultsMode.listView;
            Search(true);
        };
    }
    if (tableViewButton !== null) {
        tableViewButton.onclick = () => {
            viewMode = SearchResultsMode.tableView;
            Search(true);
        };
    }
    if (context)
        context(context__);
    Search(true);
    return context__;
}
async function WWCPGetCollection(requestURL, doStartUp, nameOfItems, doStatistics, doFinish) {
    // Full implementation can be added later – the important parts are already typed correctly
    console.warn("WWCPGetCollection – implementation stub");
}
// Legacy functions
function HTTP(Method, RessourceURI, Data, OnSuccess, OnError) {
    const ajax = new XMLHttpRequest();
    ajax.open(Method, RessourceURI, true);
    ajax.setRequestHeader("Accept", "application/json; charset=UTF-8");
    ajax.setRequestHeader("Content-Type", "application/json; charset=UTF-8");
    ajax.onreadystatechange = function () {
        if (this.readyState == 4) {
            if (this.status >= 100 && this.status < 300) {
                if (OnSuccess)
                    OnSuccess(this.status, ajax.responseText);
            }
            else if (OnError)
                OnError(this.status, this.statusText, ajax.responseText);
        }
    };
    if (Data !== null)
        ajax.send(JSON.stringify(Data));
    else
        ajax.send();
}
function HTTPGet(RessourceURI, OnSuccess, OnError) {
    const ajax = new XMLHttpRequest();
    ajax.open("GET", RessourceURI, true);
    ajax.setRequestHeader("Accept", "application/json; charset=UTF-8");
    ajax.setRequestHeader("X-Portal", "true");
    ajax.onreadystatechange = function () {
        if (this.readyState == 4) {
            if (this.status >= 100 && this.status < 300) {
                if (OnSuccess)
                    OnSuccess(this.status, ajax.responseText);
            }
            else if (OnError)
                OnError(this.status, this.statusText, ajax.responseText);
        }
    };
    ajax.send();
}
function ParseJSON_LD(Text, Context = null) {
    const data = JSON.parse(Text);
    if (!Array.isArray(data))
        data["id"] = data["@id"];
    return data;
}
function GetDefaults() {
    return {
        topLeft: document.getElementById("topLeft"),
        menuVersions: document.getElementById("menuVersions"),
        menuRemoteParties: document.getElementById("menuRemoteParties")
    };
}
//# sourceMappingURL=defaults.js.map