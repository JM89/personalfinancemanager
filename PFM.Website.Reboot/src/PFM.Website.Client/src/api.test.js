import test from "node:test";
import assert from "node:assert/strict";
import { request, RequestError } from "./api.js";

test("request serializes JSON and returns parsed data", async (t) => {
  t.mock.method(globalThis, "fetch", async (url, options) => {
    assert.equal(url, "/api/frontend/accounts");
    assert.equal(options.headers.get("Content-Type"), "application/json");
    assert.deepEqual(JSON.parse(options.body), { name: "Account" });
    return new Response(JSON.stringify({ id: 1 }), { status: 200 });
  });
  assert.deepEqual(await request("/accounts", { method: "POST", body: { name: "Account" } }), { id: 1 });
});

test("request surfaces unauthorized and server errors", async (t) => {
  t.mock.method(globalThis, "fetch", async () => new Response(null, { status: 401 }));
  await assert.rejects(request("/session"), (error) => error instanceof RequestError && error.status === 401);
  t.mock.method(globalThis, "fetch", async () => new Response(JSON.stringify({ detail: "Not available" }), { status: 400, headers: { "content-type": "application/problem+json" } }));
  await assert.rejects(request("/session"), /Not available/);
});
