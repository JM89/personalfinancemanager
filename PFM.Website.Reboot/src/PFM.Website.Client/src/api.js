export class RequestError extends Error {
  constructor(message, status) {
    super(message);
    this.status = status;
  }
}

export async function request(path, options = {}) {
  const headers = new Headers(options.headers);
  if (options.body && !(options.body instanceof FormData)) {
    headers.set("Content-Type", "application/json");
    options = { ...options, body: JSON.stringify(options.body) };
  }
  const response = await fetch(`/api/frontend${path}`, { ...options, headers });
  if (response.status === 401) throw new RequestError("Please log in to continue.", 401);
  if (!response.ok) {
    const type = response.headers.get("content-type") || "";
    const error = type.includes("json") ? await response.json() : await response.text();
    const detail = typeof error === "string" ? error : error.detail || error.title || JSON.stringify(error.errors);
    throw new RequestError(detail || `Request failed (${response.status}).`, response.status);
  }
  return response.status === 204 ? null : response.json();
}
