
# Rovio Case Study – Puzzle Prototype

A color-based puzzle prototype developed as part of a Rovio case study.

This project focuses not only on the core gameplay loop, but also on building a scalable and maintainable gameplay architecture. The main goal was to keep systems decoupled, data-driven, and easy to iterate on while designing levels and mechanics.

---

## Gameplay Preview

<p align="center">
  <img src="https://github.com/user-attachments/assets/9bd85568-0994-425a-8a1f-f118a41713c8" alt="gameplay" width="320"/>
</p>


🎥 **Gameplay Video:** [Watch on YouTube](https://www.youtube.com/watch?v=1rU6ku6ZvY0&list=PLZ1QlqWyR4gPr8lmlTJAaHDDQ6DjjlKcx)

---

## Level Authoring / Data Design

Levels are authored through **ScriptableObjects**, allowing gameplay content to be created and iterated without hardcoding level data.

<p align="center">
  <img src="https://github.com/user-attachments/assets/50a765b2-708d-4254-a5a3-0c8123f1b825" alt="level-design" width="450"/>
</p>

This setup makes balancing, testing, and content iteration significantly easier by keeping **data** separate from **runtime logic**.

---

## Project Overview

This prototype was built with a strong emphasis on:

- maintainable gameplay architecture
- decoupled system communication
- data-driven level setup
- low-overhead async flow handling
- editor-friendly iteration workflow

The project demonstrates both the playable experience and the technical decisions behind the implementation.

---

## Architecture Overview

The project was built around a modular gameplay architecture with clear separation of responsibilities.

### Service Locator

I used a **Service Locator** structure to manage and access shared cross-cutting systems. This helps centralize commonly used services and reduces unnecessary direct scene references between gameplay systems.

This approach was especially useful for keeping shared structures organized and accessible without tightly coupling unrelated systems.

### EventBus / Observer Pattern

For decoupled and event-driven communication, I used an **EventBus** structure inspired by the **Observer Pattern**.

Instead of making systems directly depend on one another, gameplay events can be published and observed by interested systems. This makes the codebase easier to extend and refactor as new mechanics are added.

Examples of event-driven communication include:

- gameplay state changes
- interaction notifications
- flow-based transitions
- system-to-system updates

### Interface Segregation Between Controllers

For communication between controllers, I preferred **interface segregation** instead of exposing large concrete dependencies.

This keeps systems loosely coupled and ensures each controller depends only on the behavior it actually needs. It also improves maintainability by preventing controllers from becoming overly aware of one another’s internal implementation.

### UniTask for Async Gameplay Flow

For asynchronous gameplay flows, I used **UniTask** to keep sequencing and timed operations cleaner and more efficient.

I preferred this approach because UniTask provides a lightweight, struct-based async model that helps reduce allocation overhead compared to heavier coroutine-driven patterns in many gameplay scenarios.

This was especially useful for:

- ordered gameplay sequences
- chained async actions
- timed transitions
- non-blocking flow control

### ScriptableObjects for Data / Logic Separation

To support a more maintainable and scalable content pipeline, I used **ScriptableObjects** to separate level data and configuration from gameplay logic.

This made it easier to:

- create levels without hardcoding
- rebalance gameplay values quickly
- test content faster
- keep runtime systems cleaner

The level authoring screenshot above shows how level content can be designed directly through data assets.

### Dreamteck Splines

For spline-based path authoring and movement-related needs, I used **Dreamteck Splines**.

This provided a flexible visual workflow inside the Unity Editor and made it easier to manage guided movement paths with better iteration speed during development.

---

## Technical Decisions

### Why this architecture?

The architecture was designed to make the project:

- easier to extend with new mechanics
- safer to refactor
- faster to iterate on during level design
- cleaner in terms of controller responsibilities
- less tightly coupled across gameplay systems

### Main Development Priorities

During development, I focused on the following engineering goals:

- modular and maintainable gameplay code
- event-driven communication
- interface-based controller interaction
- data-driven content production
- cleaner async sequencing
- editor usability during iteration

---

## Tools / Packages

- **Unity**
- **UniTask**
- **Dreamteck Splines**
- **ScriptableObjects**
- Custom **EventBus**
- **Service Locator** pattern
- **Interface-based controller communication**

---

## What This Case Demonstrates

This case study was built to demonstrate more than just a playable prototype.

It also reflects my approach to:

- structuring gameplay systems in a scalable way
- keeping dependencies under control
- building editor-friendly pipelines
- separating content from logic
- supporting rapid iteration during mechanic development

---
