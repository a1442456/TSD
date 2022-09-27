# Hw Device management

### Hw Device operations flow

1. server doesn't know about device, state requests gives 'missing' state
1. user enters server address on device
1. device requests server public key
1. server responds with server public key
1. user on device checks (using another channel) and accepts server public key
1. device sends device's public key to server
1. server checks if device exists on the server and stores it (if device exists, device gets error)
1. server stores device registration request, registration request state on server becomes 'awaiting'
1. device switches to state monitoring mode
1. user on server processes device registration request
    1. user on server accepts device's registration request on server becomes 'active'
    1. ... or user on server declines device's registration request on server becomes 'declined' 
1. user on server can lock/unlock device's public key, and this changes device state between 'active' and 'blocked'
1. user on server can delete device's public key, and device state changes to 'missing'
1. active/locked device can leave server and device state will become 'missing'
