	.intel_syntax noprefix
	.globl	XInputGetState
	.def	XInputGetState; .scl	2;	.type	32;	.endef
	
XInputGetState:

	mov eax, 0x012345678
	jmp eax
	.p2align 4,,15	
	
	
	.globl	XInputSetState
	.def	XInputSetState; .scl	2;	.type	32;	.endef
	
XInputSetState:

	mov eax, 0x012345678
	jmp eax
	.p2align 4,,15
	
	.globl	XInputGetBatteryInformation
	.def	XInputGetBatteryInformation; .scl	2;	.type	32;	.endef
	
XInputGetBatteryInformation:

	mov eax, 0x012345678
	jmp eax
	.p2align 4,,15
	
	.globl	XInputEnable
	.def	XInputEnable; .scl	2;	.type	32;	.endef
	
XInputEnable:

	mov eax, 0x012345678
	jmp eax
	.p2align 4,,15
	
	.globl	XInputGetCapabilities
	.def	XInputGetCapabilities; .scl	2;	.type	32;	.endef
	
XInputGetCapabilities:

	mov eax, 0x012345678
	jmp eax
	.p2align 4,,15
	
	.globl	XInputGetDSoundAudioDeviceGuids
	.def	XInputGetDSoundAudioDeviceGuids; .scl	2;	.type	32;	.endef
	
XInputGetKeystroke:

	mov eax, 0x012345678
	jmp eax
	.p2align 4,,15
	
	.globl	XInputGetKeystroke
	.def	XInputGetKeystroke; .scl	2;	.type	32;	.endef
	
XInputGetDSoundAudioDeviceGuids:

	mov eax, 0x012345678
	jmp eax
	.p2align 4,,15
	
	.section .drectve
	.ascii " -export:\"XInputGetState\""
	.ascii " -export:\"XInputSetState\""
	.ascii " -export:\"XInputGetBatteryInformation\""
	.ascii " -export:\"XInputEnable\""
	.ascii " -export:\"XInputGetCapabilities\""
	.ascii " -export:\"XInputGetDSoundAudioDeviceGuids\""
	.ascii " -export:\"XInputGetKeystroke\""