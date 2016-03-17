Client.Host="127.0.0.1"
Client.Port=6666
Client.Username="1"
Client.Password="1"

Client.Connect()

print ("Client version:", Client.Version)
print ("Server version:", Client.GetVersion())

local maxMfn = Client.GetMaxMfn ()
print ("Max MFN:", maxMfn)

local record = Client.ReadRecord (1)
print (record.ToIsisText())