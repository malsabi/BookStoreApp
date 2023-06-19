﻿using BookStoreApp.Views;
using BookStoreApp.Models;
using BookStoreApp.Messages;
using BookStoreApp.Services;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BookStoreApp.ViewModels
{
    public class BooksViewModel : ObservableRecipient, IRecipient<InsertNewBookMessage>
    {
        private readonly IBookService bookService;

        public ObservableCollection<Book> BooksList { get; } = new ObservableCollection<Book>();

        public IRelayCommand NavigateToAddBookCommand { get; }

        public BooksViewModel(IBookService bookService)
        {
            this.bookService = bookService;
            NavigateToAddBookCommand = new RelayCommand(NavigateToAddBook);
            IsActive = true;

            if (bookService.GetBooks() is List<Book> books)
            {
                foreach (Book book in books)
                {
                    BooksList.Add(book);
                }
            }
        }

        private async void NavigateToAddBook()
        {
            await Shell.Current.GoToAsync(nameof(AddBookPage), true);
        }

        protected override void OnActivated()
        {
            WeakReferenceMessenger.Default.Register(this);
        }

        protected override void OnDeactivated()
        {
            WeakReferenceMessenger.Default.Unregister<InsertNewBookMessage>(this);
        }

        public void Receive(InsertNewBookMessage message)
        {
            BooksList.Add(message.Value);
        }
    }
}