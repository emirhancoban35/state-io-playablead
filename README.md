# State.io - Playable Ad Prototype ⚔️

A high-performance, MRAID-ready playable ad prototype inspired by State.io, developed with a strict focus on mobile web optimization and scalable architecture.

## 🧠 Core Architecture & Technical Highlights
* **Zero-GC Mathematics:** Replaced heavy Unity Physics (RigidBody/Colliders) with pure `sqrMagnitude` calculations. Zero garbage collection during frame updates.
* **Event-Driven UI (Layered Fill):** Eliminated costly `Update` polling. The tri-color progress bar operates strictly on C# `Action` events triggered only when nodes change ownership.
* **MRAID & Telemetry Ready:** Built-in `.jslib` bridge for pure HTML5/JavaScript communication. 
    * Secured event tracking (Impression, First Interaction, Level Win/Lose).
    * Auto-redirect functionality via `mraid.open(url)` without heavy UI layers.
* **Optimized Object Pooling:** Seamless unit spawning and recycling, keeping RAM usage strictly linear.
