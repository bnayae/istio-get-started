/**********************************************************
 * Copyright 2019 Bnaya Eshet ©
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 *********************************************************** */

const express = require("express");
const fetch = require("node-fetch");
const cors = require("cors");
const timeout = require("await-timeout");

const PORT = process.env.PORT || 80;
const TARGET_URL = process.env.TARGET_URL || "http://bnaya-ping-service";

const app = express();

app.use(cors());

app.get("/health", (req, res, next) => {
  res.send("pong is alive");
});

app.get("/ready", async (req, res, next) => {
  await timeout.set(3000);
  res.send("pong is ready");
});

app.get("/", async (req, res, next) => {
  let count = req.query.count;
  count -= 1;
  let data = [`JS v1: ${"-".repeat(count)}|`];
  if (count <= 1) {
    res.json(data);
  } else {
    try {
      let response = await fetch(`${TARGET_URL}?count=${count}`, {
        mode: "no-cors"
      });
      let json = await response.json();
      res.json(json.concat(data));
    } catch (ex) {
      console.log(ex);
      res.json(json.concat(["JS Error"]));
    }
  }
});

app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
