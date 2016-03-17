Client.Host="127.0.0.1"
Client.Port=6666
Client.Username="miron"
Client.Password="miron"

Client.Connect()

print ("Client version:", Client.Version)
print ("Server version:", Client.GetVersion())

local maxMfn = Client.GetMaxMfn ()
print ("Max MFN:", maxMfn)