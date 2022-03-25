# The game framework your mother warned you about.

Godot 4 Fast-Paced (Authoritative Server) Game + Network Framework for FPS and TPS Games

# Network features
- Client-side prediction of player entities
- Client-side interpolation of remote entities
- Backwards reconciliation and replay
- Real-time adjustment of client simulation speed to optimize server's input buffer (Overwatch's method).
- Server-side lag compensation
- (TODO) Hitscan weapons
- (TODO) Projectile weapons
- Master server in godot (without 3d)
- Master and multi clients in one project (split screen)
- Optimized netcode (Quake, Overwatch, Valve methods)

# Physics
- Full implemented TPS and FPS Movement (Quake Like)
- Crouching
- (TODO) Shifting
- (TODO) Lieing
- (TODO) Animation network sync

# Helpers
- Component system (for extending characters and game world)
- Registration services (Full threaded services like Networking)
- Async world loader
- (TODO) Async mesh loader

# Utils
- Simple logging
