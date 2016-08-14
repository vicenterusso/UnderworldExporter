using UnityEngine;
using System.Collections;

public class Boulder : object_base {

	public override bool ActivateByObject (GameObject ObjectUsed)
	{
				if (ObjectUsed.GetComponent<ObjectInteraction>().item_id==296)
				{//Bashed with a rock hammer
						ObjectInteraction newObj;
						switch (objInt().item_id)
						{
						case 339://Large Boulders
						case 340://Split into two boulders
							for (int i=0;i<2;i++)
							{
								newObj= ObjectInteraction.CreateNewObject(341);	
								if (newObj!=null)
								{
									newObj.gameObject.transform.position=this.transform.position+new Vector3(Random.Range(-0.6f,0.6f),0.0f,Random.Range(-0.6f,0.6f));
									newObj.gameObject.transform.parent=GameWorldController.instance.LevelMarker();									
								}
								Destroy(this.gameObject);
							}
							break;
						case 341://Boulder. //Split into 2 small boulders
							for (int i=0;i<2;i++)
							{
								newObj= ObjectInteraction.CreateNewObject(342);	
								if (newObj!=null)
								{
									newObj.gameObject.transform.position=this.transform.position+new Vector3(Random.Range(-0.6f,0.6f),0.0f,Random.Range(-0.6f,0.6f));
									newObj.gameObject.transform.parent=GameWorldController.instance.LevelMarker();									
								}
								Destroy(this.gameObject);
							}
							break;
						case 342://Small boulder
							//Split into random qty of slingstones
							newObj= ObjectInteraction.CreateNewObject(16);	
							newObj.Link= Random.Range(3,10);
							newObj.isQuant=true;
							newObj.gameObject.transform.position=this.transform.position;
							newObj.gameObject.transform.parent=GameWorldController.instance.LevelMarker();
							Destroy(this.gameObject);
							break;
						}
						return true;
				}
				else
				{
						return base.ActivateByObject (ObjectUsed);				
				}
		
	}

}
