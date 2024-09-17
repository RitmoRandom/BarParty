using BarParty.Models;
using SQLite;

namespace BarParty
{
    public class LocalDbService
    {
       private const string DB_NAME = "BarParty.db3";
        private readonly SQLiteAsyncConnection _connection;
        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<Cups>();
            _connection.CreateTableAsync<Methods>();
            _connection.CreateTableAsync<Sizes>();
            _connection.CreateTableAsync<Tags>();
            _connection.CreateTableAsync<TagType>();
            _connection.CreateTableAsync<Ingredients>();
            _connection.CreateTableAsync<TypeIng>();

            _connection.CreateTableAsync<IngXRec>();
            _connection.CreateTableAsync<Recipe>();
            _connection.CreateTableAsync<TagXRec>();

            _connection.CreateTableAsync<Party>();
            _connection.CreateTableAsync<CoctelxParty>();

        }
        public async void fav()
        {
            string directorioLocal = Path.Combine(AppContext.BaseDirectory, "Cimagenes");

            // Verificar si el directorio existe, si no, créalo
            if (!Directory.Exists(directorioLocal))
            {
                Directory.CreateDirectory(directorioLocal);
            }
            TagType favid = await _connection.Table<TagType>().Where(x => x.Name == "Fav").FirstOrDefaultAsync();
            if (favid!=null)
            {
                Tags tt = await _connection.Table<Tags>().Where(x => x.tipo == favid.Id).FirstOrDefaultAsync();
                if (tt == null)
                {
                    await _connection.InsertAsync(new Tags { Name = "Fav", tipo = favid.Id });
                }
            }
            else
            {
                await _connection.InsertAsync(new TagType { Name="Fav"});
                favid = await _connection.Table<TagType>().Where(x => x.Name == "Fav").FirstAsync();
                await _connection.InsertAsync(new Tags { Name="Fav",tipo=favid.Id});
            }
        }
        public async void import()
        {
            TagType favid = await _connection.Table<TagType>().Where(x => x.Name == "Fav").FirstOrDefaultAsync();
            if (favid != null)
            {
                
                Tags tt = await _connection.Table<Tags>().Where(x => x.tipo == favid.Id).FirstOrDefaultAsync();
                if (tt != null)
                {
                    await _connection.DeleteAsync(favid);
                    await _connection.DeleteAsync(tt);
                }
            }
        }
        public async Task<List<Cups>> GetCups()
        {

            return await _connection.Table<Cups>().ToListAsync();
        }
        public async Task<Cups> GetCupById(int id)
        {
            return await _connection.Table<Cups>().Where(x => x.Id==id).FirstOrDefaultAsync();
        }
        public async Task CreateCup(Cups cup)
        {
            await _connection.InsertAsync(cup);
        }
        public async Task UpdateCup(Cups cup)
        {
            await _connection.UpdateAsync(cup);
        }
        public async Task DeleteCup(Cups cup)
        {
            await _connection.DeleteAsync(cup);
        }


        public async Task<List<Methods>> GetMethods()
        {

            return await _connection.Table<Methods>().ToListAsync();
        }
        public async Task<Methods> GetMethodById(int id)
        {
            return await _connection.Table<Methods>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task CreateMethod(Methods meth)
        {
            await _connection.InsertAsync(meth);
        }
        public async Task UpdateMethod(Methods meth)
        {
            await _connection.UpdateAsync(meth);
        }
        public async Task DeleteMethod(Methods cup)
        {
            await _connection.DeleteAsync(cup);
        }


        public async Task<List<Sizes>> GetSizes()
        {

            return await _connection.Table<Sizes>().ToListAsync();
        }
        public async Task<Sizes> GetSizeById(int id)
        {
            return await _connection.Table<Sizes>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task CreateSize(Sizes s)
        {
            await _connection.InsertAsync(s);
        }
        public async Task UpdateSize(Sizes s)
        {
            await _connection.UpdateAsync(s);
        }
        public async Task DeleteSize(Sizes cup)
        {
            await _connection.DeleteAsync(cup);
        }


        public async Task<List<Tags>> GetTags()
        {

            return await _connection.Table<Tags>().ToListAsync();
        }
        public async Task<List<Tags>> GetTagByName(string name)
        {
            return await _connection.Table<Tags>().Where(x => (x.Name.ToUpper()).Contains(name.ToUpper())).ToListAsync();
        }
        public async Task<Tags> GetTagById(int id)
        {
            return await _connection.Table<Tags>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task CreateTag(Tags t)
        {
            await _connection.InsertAsync(t);
        }
        public async Task UpdateTag(Tags t)
        {
            await _connection.UpdateAsync(t);
        }
        public async Task DeleteTag(Tags cup)
        {
            await _connection.DeleteAsync(cup);
        }


        public async Task<List<TagType>> GetTypeTags()
        {
            return await _connection.Table<TagType>().ToListAsync();
        }
        public async Task<TagType> GetTypeTagsByName(string Name)
        {
            return await _connection.Table<TagType>().Where(x => x.Name.Contains(Name)).FirstAsync();
        }
        public async Task<TagType> GetTypeTagById(int id)
        {
            return await _connection.Table<TagType>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task CreateTypeTag(TagType t)
        {
            await _connection.InsertAsync(t);
        }
        public async Task UpdateTypeTag(TagType t)
        {
            await _connection.UpdateAsync(t);
        }
        public async Task DeleteTypeTag(TagType cup)
        {
            await _connection.DeleteAsync(cup);
        }



        public async Task<List<Ingredients>> GetIngredients()
        {

            return await _connection.Table<Ingredients>().ToListAsync();
        }
        public async Task<Ingredients> GetIngredientById(int id)
        {
            return await _connection.Table<Ingredients>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Ingredients>> GetIngredientFiltName(string name)
        {
            return await _connection.Table<Ingredients>().Where(x => (x.Name.ToUpper()).Contains(name.ToUpper())).ToListAsync();
        }
        public async Task CreateIngredient(Ingredients i)
        {
            await _connection.InsertAsync(i);
        }
        public async Task UpdateIngredients(Ingredients i)
        {
            await _connection.UpdateAsync(i);
        }
        public async Task DeleteIngredient(Ingredients cup)
        {
            await _connection.DeleteAsync(cup);
        }


        public async Task<List<TypeIng>> GetTypeIng()
        {

            return await _connection.Table<TypeIng>().ToListAsync();
        }
        public async Task<TypeIng> GetTypeIngById(int id)
        {
            return await _connection.Table<TypeIng>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task CreateTypeIng(TypeIng ti)
        {
            await _connection.InsertAsync(ti);
        }
        public async Task UpdateTypeIng(TypeIng ti)
        {
            await _connection.UpdateAsync(ti);
        }
        public async Task DeleteTypeIng(TypeIng cup)
        {
            await _connection.DeleteAsync(cup);
        }
        

        public async Task<List<Recipe>> GetRecipes()
        {

            return await _connection.Table<Recipe>().ToListAsync();
        }
        public async Task<Recipe> GetRecipeById(int id)
        {
            return await _connection.Table<Recipe>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Recipe>> GetRecipeFiltName(string name)
        {
            return await _connection.Table<Recipe>().Where(x => (x.Name.ToUpper()).Contains(name.ToUpper())).ToListAsync();
        }
        public async Task CreateRecipe(Recipe ti)
        {
            await _connection.InsertAsync(ti);
        }
        public async Task UpdateRecipe(Recipe ti)
        {
            await _connection.UpdateAsync(ti);
        }
        public async Task DeleteRecipe(Recipe cup)
        {
            await _connection.DeleteAsync(cup);
        }
        
        public async Task<List<IngXRec>> GetIngXRecById(int id)
        {
                return await _connection.Table<IngXRec>().Where(x => x.RecID == id).ToListAsync();
        }
        public async Task CreateIngXRec(IngXRec ti)
        {
            await _connection.InsertAsync(ti);
        }
        public async Task UpdateIngXRec(IngXRec ti)
        {
            await _connection.UpdateAsync(ti);
        }
        public async Task DeleteIngXRec(IngXRec cup)
        {
            await _connection.DeleteAsync(cup);
        }


        public async Task<List<TagXRec>> GetTagXRecById(int id)
        {
            return await _connection.Table<TagXRec>().Where(x => x.RecID == id).ToListAsync();
        }
        public async Task CreateTagXRec(TagXRec ti)
        {
            await _connection.InsertAsync(ti);
        }
        public async Task UpdateTagXRec(TagXRec ti)
        {
            await _connection.UpdateAsync(ti);
        }
        public async Task DeleteTagXRec(TagXRec cup)
        {
            await _connection.DeleteAsync(cup);
        }


        public async Task<List<Party>> GetPartys()
        {
            return await _connection.Table<Party>().ToListAsync();
        }
        public async Task<Party> GetPartyById(int id)
        {
            return await _connection.Table<Party>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task CreateParty(Party ti)
        {
            await _connection.InsertAsync(ti);
        }
        public async Task UpdateParty(Party ti)
        {
            await _connection.UpdateAsync(ti);
        }
        public async Task DeleteParty(Party cup)
        {
            await _connection.DeleteAsync(cup);
        }


        public async Task<List<CoctelxParty>> GetCoctelxPartyById(int id)
        {
            return await _connection.Table<CoctelxParty>().Where(x => x.PartyID == id).ToListAsync();
        }
        public async Task CreateCoctelxParty(CoctelxParty ti)
        {
            await _connection.InsertAsync(ti);
        }
        public async Task UpdateCoctelxParty(CoctelxParty ti)
        {
            await _connection.UpdateAsync(ti);
        }
        public async Task DeleteCoctelxParty(CoctelxParty cup)
        {
            await _connection.DeleteAsync(cup);
        }
    }
}
