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

// https://expressjs.com/en/guide/routing.html

const express = require("express");
const fetch = require("node-fetch");
const cors = require("cors");
const timeout = require("await-timeout");

const PORT = process.env.PORT || 80;

const app = express();

app.use(cors());

app.get("/*", async (req, res, next) => {
  res.send(`
  <dir>
  <h1>Echo</h1>
  <p>Params: ${JSON.stringify(req.params)}</p>
  <p>Query:  ${JSON.stringify(req.query)}</p>
  <h3>Node Express</h3>
  <p>Pod Language: Javascript (express)</p>
  <p>Pod Version: 2</p>
  <div>`);
});

app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
