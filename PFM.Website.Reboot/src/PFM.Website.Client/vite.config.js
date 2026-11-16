import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5174,
    strictPort: true,
    proxy: {
      "/api": "http://localhost:5110",
      "/login": "http://localhost:5110",
      "/logout": "http://localhost:5110",
    },
  },
});
