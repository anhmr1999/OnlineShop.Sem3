namespace Shop.EntityFramework.Migrations
{
    using Shop.EntityFramework.Common;
    using Shop.EntityFramework.Entities;
    using Shop.EntityFramework.Infrastructures.Permissions;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Shop.EntityFramework.ShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Shop.EntityFramework.ShopDbContext context)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Admin Manager",
                Username = "manager",
                Email = "manager@shop.com",
                Phone = "0123456789",
                Password = "1q2w3E*".ToMd5(),
                IsActive = true,
                CreationTime = DateTime.Now,
                Roles = new List<Role>()
            };
            var role = new Role()
            {
                Id = Guid.NewGuid(),
                RoleName = "manager",
                IsStatic = true,
                IsDefault = false,
                CreationTime = DateTime.Now
            };
            user.Roles.Add(role);
            var permissionGrants = GetPermssionGrant(PermissionProvider.Permissions);

            context.Roles.Add(role);
            context.PermissionGrants.AddRange(permissionGrants);
            context.SaveChanges();
            context.Users.Add(user);
            context.Categories.AddRange(new List<Category>() { 
                new Category(){Id = Guid.NewGuid(), CateFor = null, Code = "action", Name = "Action film", CreationTime = DateTime.Now, Description = "Phim hành động là một thể loại phim trong đó nhân vật chính bị đẩy vào một loạt các sự kiện thường liên quan đến bạo lực và chiến công thể chất. Thể loại này có xu hướng kể về một anh hùng chủ yếu là tháo vát đấu tranh chống lại những khó khăn đáng kinh ngạc, bao gồm các tình huống nguy hiểm đến tính mạng, một kẻ ác nguy hiểm hoặc một cuộc truy đuổi thường kết thúc bằng chiến thắng cho anh hùng."},
                new Category(){Id = Guid.NewGuid(), CateFor = null, Code = "romance", Name = "Romance film", CreationTime = DateTime.Now, Description = "Romance films or movies involve romantic love stories recorded in visual media for broadcast in theatres or on television that focus on passion, emotion, and the affectionate romantic involvement of the main characters. Typically their journey through dating, courtship or marriage is featured. These films make the search for romantic love the main plot focus. Occasionally, romance lovers face obstacles such as finances, physical illness, various forms of discrimination, psychological restraints or family resistance. As in all quite strong, deep and close romantic relationships, the tensions of day-to-day life, temptations (of infidelity), and differences in compatibility enter into the plots of romantic films."},
                new Category(){Id = Guid.NewGuid(), CateFor = null, Code = "horror", Name = "Horror film", CreationTime = DateTime.Now, Description = "Horror is a film genre that seeks to elicit fear or disgust in its audience for entertainment purposes."},
                new Category(){Id = Guid.NewGuid(), CateFor = false, Code = "shooter", Name = "Shooter game", CreationTime = DateTime.Now, Description = "Shooter video games or shooters are a subgenre of action video games where the focus is almost entirely on the defeat of the character's enemies using the weapons given to the player. Usually these weapons are firearms or some other long-range weapons, and can be used in combination with other tools such as grenades for indirect offense, armor for additional defense, or accessories such as telescopic sights to modify the behavior of the weapons. A common resource found in many shooter games is ammunition, armor or health, or upgrades which augment the player character's weapons."},
                new Category(){Id = Guid.NewGuid(), CateFor = false, Code = "moba", Name = "Multiplayer online battle arena (MOBA)", CreationTime = DateTime.Now, Description = "Multiplayer online battle arena (MOBA) is a subgenre of strategy video games in which two teams of players compete against each other on a predefined battlefield. Each player controls a single character with a set of distinctive abilities that improve over the course of a game and which contribute to the team's overall strategy.The typical ultimate objective is for each team to destroy their opponents' main structure, located at the opposite corner of the battlefield. In some MOBA games, the objective can be defeating every player on the enemy team. Players are assisted by computer-controlled units that periodically spawn in groups and march forward along set paths toward their enemy's base, which is heavily guarded by defensive structures. This type of multiplayer online video games originated as a subgenre of real-time strategy, though MOBA players usually do not construct buildings or units. Moreover, there are examples of MOBA games that are not considered real-time strategy games, such as Smite (2014), and Paragon. The genre is seen as a fusion of real-time strategy, role-playing and action games."},
                new Category(){Id = Guid.NewGuid(), CateFor = true, Code = "classical", Name = "Classical music", CreationTime = DateTime.Now, Description = "Classical music generally refers to the art music of the Western world, considered to be distinct from Western folk music or popular music traditions. It is sometimes distinguished as Western classical music, as the term \"classical music\" also applies to non-Western art music. Classical music is often characterized by formality and complexity in its musical form and harmonic organization, particularly with the use of polyphony.[2] Since at least the ninth century it has been primarily a written tradition, spawning a sophisticated notational system, as well as accompanying literature in analytical, critical, historiographical, musicological and philosophical practices. A foundational component of Western Culture, classical music is frequently seen from the perspective of individual or groups of composers, whose compositions, personalities and beliefs have fundamentally shaped its history."},
                new Category(){Id = Guid.NewGuid(), CateFor = true, Code = "rock-and-roll", Name = "Rock and roll music", CreationTime = DateTime.Now, Description = "Rock and roll (often written as rock & roll, rock 'n' roll, or rock 'n roll) is a genre of popular music that evolved in the United States during the late 1940s and early 1950s. It originated from African-American music such as jazz, rhythm and blues, boogie woogie, gospel, as well as country music. While rock and roll's formative elements can be heard in blues records from the 1920s and in country records of the 1930s, the genre did not acquire its name until 1954."},
                new Category(){Id = Guid.NewGuid(), CateFor = true, Code = "edm", Name = "Electronic dance music (EDM)", CreationTime = DateTime.Now, Description = "Electronic dance music (EDM), also known as dance music, club music, or simply dance, is a broad range of percussive electronic music genres made largely for nightclubs, raves, and festivals. It is generally produced for playback by DJs who create seamless selections of tracks, called a DJ mix, by segueing from one recording to another. EDM producers also perform their music live in a concert or festival setting in what is sometimes called a live PA."},
            });
            context.SaveChanges();
        }

        private ICollection<PermissionGrant> GetPermssionGrant(ICollection<Permission> permissions)
        {
            var result = new List<PermissionGrant>();
            foreach (var item in permissions)
            {
                result.Add(new PermissionGrant() { Id = Guid.NewGuid(), Name = item.Name, ProviderKey = "R", ProviderName = "manager" });
                if (item.Permissions != null && item.Permissions.Count > 0)
                    result.AddRange(GetPermssionGrant(item.Permissions));
            }
            return result;
        }
    }
}
