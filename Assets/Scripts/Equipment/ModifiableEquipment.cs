public class ModifiableEquipment : Equipment
{
    protected virtual void PerformModifierChecks() 
    {
        ModifierChecks();
    }
    protected virtual void ModifierChecks(){ }
}
