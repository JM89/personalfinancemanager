import React, { createContext, useContext, useEffect, useId, useRef, useState } from "react";

const ThemeContext = createContext(null);
const themeKey = "pfm-appearance";

export function BrandMark() {
  return <img className="brand-mark" src="/pfm-mark.svg" width="40" height="40" alt="" />;
}

export function Icon({ name, ...props }) {
  const paths = {
    overview: "M3 3h7v7H3z M14 3h7v7h-7z M3 14h7v7H3z M14 14h7v7h-7z",
    bank: "m3 9 9-6 9 6H3 M5 10v8 M10 10v8 M14 10v8 M19 10v8 M3 21h18",
    wallet: "M20 8V5H4a2 2 0 0 0 0 4h16v11H4a2 2 0 0 1-2-2V7 M20 12h-5v4h5 M17 14h.01",
    category: "M3 3h8l10 10-8 8L3 11V3 M7 7h.01",
    arrow: "M5 12h14 m-5-5 5 5-5 5",
    plus: "M12 5v14 M5 12h14",
    sun: "M12 3V1 M12 23v-2 M3 12H1 M23 12h-2 M5.6 5.6 4.2 4.2 M19.8 19.8l-1.4-1.4 M5.6 18.4l-1.4 1.4 M19.8 4.2l-1.4 1.4 M16 12a4 4 0 1 1-8 0 4 4 0 0 1 8 0",
    moon: "M21 13A9 9 0 0 1 11 3a9 9 0 1 0 10 10",
    close: "m6 6 12 12 M6 18 18 6",
    check: "m5 12 4 4L19 6",
    info: "M12 11v6 M12 7h.01 M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0",
    logout: "M9 4H4v16h5 M10 12h11 m-4-4 4 4-4 4",
    trend: "m3 17 6-6 4 4 8-10 M15 5h6v6",
    spark: "m12 3 2.5 6.5L21 12l-6.5 2.5L12 21l-2.5-6.5L3 12l6.5-2.5L12 3",
    edit: "m16 3 5 5 M4 20l5-1L21 7a2 2 0 0 0-5-5L4 14v6Z",
    delete: "M3 6h18 M9 6V3h6v3 M5 6l1 15h12l1-15 M10 10v7 M14 10v7",
    star: "m12 3 2.8 5.7 6.2.9-4.5 4.4 1.1 6.2-5.6-3-5.6 3 1.1-6.2L3 9.6l6.2-.9L12 3Z",
  };
  return <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true" {...props}><path d={paths[name] || paths.overview} /></svg>;
}

export function ThemeProvider({ children }) {
  const [theme, setTheme] = useState(() => {
    try { return localStorage.getItem(themeKey) === "midnight" ? "midnight" : "pearl"; }
    catch (error) { console.warn("PFM could not read the saved appearance.", error); return "pearl"; }
  });
  const [error, setError] = useState("");
  useEffect(() => {
    document.documentElement.dataset.theme = theme;
    try { localStorage.setItem(themeKey, theme); setError(""); }
    catch (error) { console.warn("PFM could not save appearance.", error); setError("Your theme is applied, but could not be saved in this browser."); }
  }, [theme]);
  return <ThemeContext.Provider value={{ theme, setTheme }}>{error && <p className="theme-error" role="alert">{error}</p>}{children}</ThemeContext.Provider>;
}

export const useTheme = () => useContext(ThemeContext);

export function Dialog({ title, children, onClose, className = "" }) {
  const ref = useRef(null);
  const titleId = useId();
  useEffect(() => {
    const dialog = ref.current;
    const trigger = document.activeElement;
    dialog.showModal();
    dialog.querySelector("[data-dialog-autofocus]")?.focus();
    return () => {
      dialog.close();
      if (trigger instanceof HTMLElement && trigger.isConnected) trigger.focus();
    };
  }, []);
  return <dialog ref={ref} className={className} aria-labelledby={titleId} onKeyDown={(event) => {
    if (event.key === "Escape") { event.preventDefault(); event.stopPropagation(); onClose(); }
  }} onCancel={(event) => { event.preventDefault(); onClose(); }} onClick={(event) => {
    if (event.target === event.currentTarget) {
      const bounds = event.currentTarget.getBoundingClientRect();
      if (event.clientX < bounds.left || event.clientX > bounds.right || event.clientY < bounds.top || event.clientY > bounds.bottom) onClose();
    }
  }}>
    <div className="dialog-heading"><h2 id={titleId}>{title}</h2><button type="button" className="icon-button secondary" aria-label="Close dialog" onClick={onClose}><Icon name="close" /></button></div>
    {children}
  </dialog>;
}

export function Appearance() {
  const { theme, setTheme } = useTheme();
  const [open, setOpen] = useState(false);
  return <><button className="appearance-button" onClick={() => setOpen(true)}><Icon name={theme === "pearl" ? "sun" : "moon"} /><span>Appearance</span><span className="nav-hint">{theme === "pearl" ? "Pearl" : "Midnight"}</span></button>
    {open && <Dialog title="Make yourself at home" onClose={() => setOpen(false)}>
      <p className="muted">A little more you. Choose a look for your workspace.</p>
      <div className="theme-options" role="group" aria-label="Workspace theme">
        {[["pearl", "Pearl", "A fresh start. Soft ivory and ocean teal."], ["midnight", "Midnight", "A quieter view. Deep navy and cool mint."]].map(([value, name, description]) => <button key={value} className={`theme-option ${value}`} aria-pressed={theme === value} onClick={() => setTheme(value)}>
          <span className="theme-preview"><i /><span><b /><b /><b /></span></span>
          <span className="theme-option-title">{name}{theme === value && <Icon name="check" />}</span><small>{description}</small>
        </button>)}
      </div>
      <p className="form-note"><Icon name="info" /> Saved in this browser. You can change it anytime.</p>
      <div className="actions"><button onClick={() => setOpen(false)}>Done</button></div>
    </Dialog>}
  </>;
}
