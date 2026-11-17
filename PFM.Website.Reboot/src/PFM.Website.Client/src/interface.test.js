import test, { after, afterEach, before, mock } from "node:test";
import assert from "node:assert/strict";
import React from "react";
import { renderToStaticMarkup } from "react-dom/server";
import { createServer } from "vite";

let server;
let interfaceModule;
const storageDescriptor = Object.getOwnPropertyDescriptor(globalThis, "localStorage");

before(async () => {
  server = await createServer({ server: { middlewareMode: true }, appType: "custom" });
  interfaceModule = await server.ssrLoadModule("/src/Interface.jsx");
});
after(async () => { await server?.close(); });
afterEach(() => {
  mock.restoreAll();
  if (storageDescriptor) Object.defineProperty(globalThis, "localStorage", storageDescriptor);
  else delete globalThis.localStorage;
});

for (const [saved, expected] of [[null, "Pearl"], ["pearl", "Pearl"], ["midnight", "Midnight"], ["invalid", "Pearl"]]) {
  test(`appearance uses ${expected} for saved preference ${saved}`, () => {
    Object.defineProperty(globalThis, "localStorage", { configurable: true, value: { getItem: (key) => {
      assert.equal(key, "pfm-appearance");
      return saved;
    } } });
    const { ThemeProvider, Appearance } = interfaceModule;
    const html = renderToStaticMarkup(React.createElement(ThemeProvider, null, React.createElement(Appearance)));
    assert.ok(html.includes(`<span class="nav-hint">${expected}</span>`));
    assert.ok(html.includes("Appearance"));
  });
}

test("unavailable browser storage reports a warning and renders the default theme", () => {
  const failure = new Error("Storage is blocked");
  Object.defineProperty(globalThis, "localStorage", { configurable: true, get() { throw failure; } });
  const warning = mock.method(console, "warn", () => {});
  const { ThemeProvider, Appearance } = interfaceModule;
  const html = renderToStaticMarkup(React.createElement(ThemeProvider, null, React.createElement(Appearance)));
  assert.ok(html.includes('<span class="nav-hint">Pearl</span>'));
  assert.equal(warning.mock.callCount(), 1);
  assert.equal(warning.mock.calls[0].arguments[1], failure);
});

test("dialogs have a programmatically associated heading and a named close control", () => {
  const { Dialog } = interfaceModule;
  const html = renderToStaticMarkup(React.createElement(Dialog, { title: "Configure workspace", onClose() {} }, "Details"));
  const headingId = html.match(/<h2 id="([^"]+)">Configure workspace<\/h2>/)?.[1];
  assert.ok(headingId);
  assert.ok(html.includes(`aria-labelledby="${headingId}"`));
  assert.ok(html.includes('aria-label="Close dialog"'));
});

test("the workspace mark uses the shared piggy-bank vector without duplicating the wordmark label", () => {
  const html = renderToStaticMarkup(React.createElement(interfaceModule.BrandMark));
  assert.ok(html.includes('src="/pfm-mark.svg"'));
  assert.ok(html.includes('alt=""'));
  assert.ok(html.includes('width="40"'));
  assert.ok(html.includes('height="40"'));
});
