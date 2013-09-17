import namespaces from imports.boo

simulate:

	Vehicles = (VehicleControllerControl( \
							Vehicle:Gpucc(Id:2,Name:"GP02",PositionalData:MotionalData(x=2,y=2,z=0,yaw=0,pitch=0,roll=0)), \
							Controller:PurpursuitController(), \
							Control:GpuccControl()),
					VehicleControllerControl( \
							Vehicle:Gpucc(Id:1,Name:"GP01",PositionalData:MotionalData(x=1,y=-2,z=0,yaw=1.57,pitch=0,roll=0)), \
							Controller:PurpursuitController(), \
							Control:GpuccControl())	
					)