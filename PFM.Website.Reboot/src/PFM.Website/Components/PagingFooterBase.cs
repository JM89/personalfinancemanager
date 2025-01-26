using System.Reflection;
using Microsoft.AspNetCore.Components;
using PFM.Models;

namespace PFM.Website.Components
{
    public class PagingFooterBase<T> : ComponentBase
    {
        public PagedModel<T>? Model { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int MaxPage { get; set; }

        [Parameter]
        public int PageSize { get; set; }

        [Parameter]
        public EventCallback<int> FetchDataCallback { get; set; }

        protected async Task PreviousPage()
        {
            if (CurrentPage < 2)
            {
                return;
            }

            CurrentPage--;
            await FetchPagedData();
        }

        protected async Task NextPage()
        {
            if (CurrentPage > MaxPage)
            {
                return;
            }

            CurrentPage++;
            await FetchPagedData();
        }

        protected async Task ShowPage(int targetPage)
        {
            CurrentPage = targetPage;
            await FetchPagedData();
        }

        protected async Task FetchPagedData()
        {
            var skip = CurrentPage * PageSize - PageSize - 1;

            await FetchDataCallback.InvokeAsync(skip);
        }

        public void RefreshModel(PagedModel<T> model)
        {
            Model = model;
            MaxPage = Model.Count / PageSize + 1;
            this.StateHasChanged();
        }
    }
}

