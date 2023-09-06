#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.Void UnityEngine.Grid::set_cellSize(UnityEngine.Vector3)
extern void Grid_set_cellSize_mE29358F6FF9FF8E7082DC41326753807EC13EA8C (void);
// 0x00000002 System.Void UnityEngine.Grid::set_cellSize_Injected(UnityEngine.Vector3&)
extern void Grid_set_cellSize_Injected_mD04C19848B5236390ABC3CEC59EAC0D059AA4C7A (void);
// 0x00000003 System.Void UnityEngine.GridLayout::DoNothing()
extern void GridLayout_DoNothing_m36201F4787709460C994921B16CAC95CD490E0A1 (void);
static Il2CppMethodPointer s_methodPointers[3] = 
{
	Grid_set_cellSize_mE29358F6FF9FF8E7082DC41326753807EC13EA8C,
	Grid_set_cellSize_Injected_mD04C19848B5236390ABC3CEC59EAC0D059AA4C7A,
	GridLayout_DoNothing_m36201F4787709460C994921B16CAC95CD490E0A1,
};
static const int32_t s_InvokerIndices[3] = 
{
	1279,
	1195,
	1482,
};
extern const CustomAttributesCacheGenerator g_UnityEngine_GridModule_AttributeGenerators[];
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_UnityEngine_GridModule_CodeGenModule;
const Il2CppCodeGenModule g_UnityEngine_GridModule_CodeGenModule = 
{
	"UnityEngine.GridModule.dll",
	3,
	s_methodPointers,
	s_InvokerIndices,
	0,
	NULL,
	0,
	NULL,
	0,
	NULL,
	NULL,
	g_UnityEngine_GridModule_AttributeGenerators,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};
