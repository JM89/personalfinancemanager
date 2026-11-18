import test from "node:test";
import assert from "node:assert/strict";

const base = process.env.PFM_LIVE_URL;

test("the published live site serves React and protects its finance endpoints", { skip: !base }, async () => {
  const deepLink = await fetch(`${base}/expenses/list/1`);
  assert.equal(deepLink.status, 200);
  const html = await deepLink.text();
  assert.match(html, /<div id="root">/);
  const asset = html.match(/src="(\/assets\/[^"]+\.js)"/)?.[1];
  assert.ok(asset);
  assert.equal((await fetch(`${base}${asset}`)).status, 200);

  const mode = await fetch(`${base}/api/frontend/mode`);
  assert.deepEqual(await mode.json(), { offline: false });
  for (const path of ["/session", "/accounts", "/accounts/1/dashboard"]) {
    const response = await fetch(`${base}/api/frontend${path}`, { redirect: "manual" });
    assert.equal(response.status, 401);
    assert.equal(response.headers.get("location"), null);
  }
  assert.equal((await fetch(`${base}/api/frontend/no-such-route`)).status, 404);
});
